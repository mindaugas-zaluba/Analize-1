using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Analize.Array
{
    public class FileArray : IArray
    {
        const int bufSize = sizeof(int);

        private FileStream MyFile;
        public int ArraySize;

        public int Count()
        {
            return ArraySize;
        }

        public FileArray()
        {
        }

        public FileArray(string fileName)
        {
            Initialize(fileName);
        }

        public IArray Initialize(string fileName)
        {
            if (File.Exists(fileName))
            {
                MyFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ArraySize = ReadAt(-1);
            }
            else
            {
                MyFile = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
                ArraySize = 0;
                MyFile.Write(BitConverter.GetBytes(0),0, bufSize);
            }
            return this;
        }

        public void InsertAt(int index, int value)
        {
            if (index > ArraySize) { throw new IndexOutOfRangeException(); }
            if (index == ArraySize)
            {
                ArraySize++;
                ReplaceAt(ArraySize-1, value);
            }
            else
            {
                int currIndex = index;
                int prevValue = 0;
                if (index != 0)
                {
                    prevValue = ReadAt(currIndex);
                }
                else
                {
                    MoveToStart();
                    prevValue = ReadNext();
                }
                ReplaceAt(currIndex, value);
                currIndex++;
                ArraySize++;


                while (currIndex != ArraySize - 1)
                {
                    value = prevValue;
                    prevValue = ReadAt(currIndex);
                    currIndex++;
                    ReplaceAt(currIndex - 1, value);


                }
                ReplaceAt(ArraySize - 1, prevValue);

            }
        }

        public void RemoveAt(int index)
        {
            if (index > ArraySize - 1) throw new IndexOutOfRangeException();
            if (index != ArraySize - 1)
            {

                var curIndex = index;
                while (curIndex != ArraySize - 2)
                {
                    ReplaceAt(curIndex, ReadAt(curIndex + 1));
                    curIndex++;
                }
                ReplaceAt(curIndex, ReadAt(curIndex + 1));
            }
            ArraySize--;
            MyFile.SetLength(MyFile.Length - bufSize);
        }


        public void Push(int val)
        {
            InsertAt(ArraySize, val);

        }


        public IArray GetRange(int start, int length, string fileName)
        {
            var result = new FileArray(fileName);
            for (int i = start; i < start+length; i++)
            {
                result.Push(ReadAt(i));
            }
            return result;
        }


        public int ReadAt(long index)
        {
            if (index > ArraySize - 1) throw new IndexOutOfRangeException();
            var intBuffer = new byte[bufSize];
            MyFile.Seek((index + 1) * bufSize, SeekOrigin.Begin);
            MyFile.Read(intBuffer, 0, bufSize);
            return BitConverter.ToInt32(intBuffer, 0);
        }

        public void ReplaceAt(long index, int value)
        {
            if (index > ArraySize - 1) throw new IndexOutOfRangeException();
            var intBuffer = BitConverter.GetBytes(value);
            MyFile.Seek((index + 1) * bufSize, SeekOrigin.Begin);
            MyFile.Write(intBuffer, 0, bufSize);
        }

        public void MoveToStart()
        {
            MyFile.Seek(bufSize, SeekOrigin.Begin);
        }

        public int ReadNext()
        {
            try
            {
                var intBuffer = new byte[bufSize];
                int n = MyFile.Read(intBuffer, 0, bufSize);
                if (n == 0) throw new EndOfStreamException();
                return BitConverter.ToInt32(intBuffer, 0);
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
