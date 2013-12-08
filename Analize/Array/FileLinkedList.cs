using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Analize.Array
{
    public class FileLinkedList : IArray
    {
        const int bufSize = sizeof(int);

        private FileStream MyFile;
        public int ArraySize;
        public long CurrentAddress;
        public int Count()
        {
            return ArraySize;
        }
        public FileLinkedList()
        {

        }
        public FileLinkedList(string fileName)
        {
            Initialize(fileName);
        }
        public IArray Initialize(string fileName)
        {
            if (File.Exists(fileName))
            {
                MyFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var intBuffer = new byte[bufSize];
                MyFile.Read(intBuffer, 0, bufSize);
                ArraySize = BitConverter.ToInt32(intBuffer, 0);
                MoveToStart();


            }
            else
            {
                MyFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var intBuffer = BitConverter.GetBytes(0);
                MyFile.Write(intBuffer, 0, bufSize);
                ArraySize = 0;
                MoveToStart();
            }
            return this;
        }

        public void InsertAt(int index, int value)
        {
            if (index < ArraySize)
            {
                int prevValue = ReadAt(index);
                long nextAddress = CurrentAddress;
                ReplaceAt(index, value);
                var intBuffer = BitConverter.GetBytes(MyFile.Length);
                MyFile.Write(intBuffer, 0, bufSize);
                MyFile.Seek(0, SeekOrigin.End);
                intBuffer = BitConverter.GetBytes(prevValue);
                MyFile.Write(intBuffer, 0, bufSize);
                intBuffer = BitConverter.GetBytes(nextAddress);
                MyFile.Write(intBuffer, 0, bufSize);
                ArraySize++;
            }
            else if (index == ArraySize)
            {
                var intBuffer = new byte[bufSize];
                if (ArraySize == 1)
                {
                    MyFile.Seek(bufSize*3, SeekOrigin.Begin);
                    intBuffer = BitConverter.GetBytes(MyFile.Length);
                    MyFile.Write(intBuffer, 0, bufSize);
                }
                else if (ArraySize >= 1)
                {

                    ReadAt(index - 2);
                    MyFile.Seek(bufSize, SeekOrigin.Current);
                    intBuffer = BitConverter.GetBytes(MyFile.Length);
                    MyFile.Write(intBuffer, 0, bufSize);


                }
                else
                {
                    MyFile.Seek(0, SeekOrigin.Begin);
                    intBuffer = BitConverter.GetBytes(1);
                    MyFile.Write(intBuffer, 0, bufSize);
                    intBuffer = BitConverter.GetBytes(bufSize * 2);
                    MyFile.Write(intBuffer, 0, bufSize);
                }


                intBuffer = BitConverter.GetBytes(value);
                MyFile.Write(intBuffer, 0, bufSize);
                intBuffer = BitConverter.GetBytes(0);
                MyFile.Write(intBuffer, 0, bufSize);
                ArraySize++;
                ReadAt(ArraySize - 1);
            }

        }

        public void RemoveAt(int index)
        {
            if (index == 0)
            {
                ReadAt(0);
                MyFile.Seek(bufSize, SeekOrigin.Begin);
                MyFile.Write(BitConverter.GetBytes(CurrentAddress), 0, bufSize);
            }
            else
            {
                ReadAt(index);
                var newAddress = CurrentAddress;
                ReadAt(index - 1);
                MyFile.Seek(-bufSize, SeekOrigin.Current);
                MyFile.Write(BitConverter.GetBytes(newAddress), 0, bufSize);
                ReadAt(index - 1);
                ArraySize--;
            }
        }

        public void Push(int val)
        {
            InsertAt(ArraySize, val);
        }


        public IArray GetRange(int start, int length, string fileName)
        {
            var result = new FileLinkedList(fileName);
            result.Push(ReadAt(start));
            for (var i = start + 1; i < start+length; i++)
            {
                result.Push(ReadNext());
            }
            return result;
        }

        public int ReadAt(long index)
        {
            if (index > ArraySize || index < 0) throw new IndexOutOfRangeException();
            var readValue = 0;
            MoveToStart();
            int currentIndex = 0;
            readValue = ReadNext();
            while (currentIndex != index)
            {
                readValue = ReadNext();
                currentIndex++;
            }
            return readValue;
        }

        public void ReplaceAt(long index, int value)
        {
            if (index > ArraySize) throw new IndexOutOfRangeException();
            if (index == 0)
            {
                MyFile.Seek(bufSize * 2, SeekOrigin.Begin);
            }
            else
            {
                ReadAt(index - 1);
            }
            var intBuffer = BitConverter.GetBytes(value);
            MyFile.Write(intBuffer, 0, bufSize);
        }

        public void MoveToStart()
        {
            MyFile.Seek(bufSize, SeekOrigin.Begin);
            var intBuffer = new byte[bufSize];
            MyFile.Read(intBuffer, 0, bufSize);
            CurrentAddress = BitConverter.ToInt32(intBuffer, 0);
            MyFile.Seek(CurrentAddress, SeekOrigin.Begin);

        }

        public int ReadNext()
        {
            try
            {
                if (CurrentAddress == 0) throw new EndOfStreamException();
                MyFile.Seek(CurrentAddress, SeekOrigin.Begin);
                var intBuffer = new byte[bufSize];
                int n = MyFile.Read(intBuffer, 0, bufSize);
                int readValue = BitConverter.ToInt32(intBuffer, 0);
                MyFile.Read(intBuffer, 0, bufSize);
                CurrentAddress = BitConverter.ToInt32(intBuffer, 0);
                if (n == 0) throw new EndOfStreamException();
                return readValue;
            }
            catch (EndOfStreamException ex)
            {
                throw new EndOfStreamException();
            }

        }

        public void Dispose()
        {
            MyFile.Close();
        }
        public void SelfDestruct()
        {
            var fileName = MyFile.Name;
            this.Dispose();
            File.Delete(fileName);
        }
    }
}
