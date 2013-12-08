using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Analize.Array;

namespace Analize
{
    class Program
    {
        const int maxValue = 1000;
        private const int bufSize = sizeof(int);

        public enum ArrayType
        {
            LinkedList = 1,
            FileArray = 2
        }
        public enum SortType
        {
            QuickSort = 1,
            MergeSort = 2,
            CountingSort = 3
        }
        static void Main(string[] args)
        {

            var method = SortType.CountingSort;
            var arrayType = ArrayType.LinkedList;
            var output = false;
            var sampleSize = 10;

            //Console.WriteLine("Enter Sort Method (1 - QuickSort, 2 - MergeSort, 3 - CountingSort):");
            //SortType.TryParse(Console.ReadLine(), out method);

            //Console.WriteLine("Enter data type to be used(1 - LinkedList, 2 - Array):");
            //ArrayType.TryParse(Console.ReadLine(), out arrayType);

            Console.WriteLine("Array size to be generated:");
            int.TryParse(Console.ReadLine(), out sampleSize);

            //Console.WriteLine("Print out array? (Y/N)");
            //if (String.Compare(Console.ReadLine(), "Y", StringComparison.InvariantCultureIgnoreCase) == 0) output = true;
            

            switch (arrayType)
            {
                case ArrayType.LinkedList:
                    generateLinkedListFile("test.txt", sampleSize);
                    break;
                case ArrayType.FileArray:
                    generateArrayFile("test.txt", sampleSize);
                    break;
            }

            using (IArray fileArray = CreateArray(arrayType, "test.txt"))
            {
                if (output)
                {
                   printArray(fileArray);
                }
                var sortedArray = fileArray;
                var startTime= DateTime.Now;
                var operations = 0;
                switch (method)
                {
                    case SortType.QuickSort:
                        var sorter = new QuickSorter();
                        sorter.Quicksort(fileArray, 0, fileArray.Count() - 1);
                        operations = sorter.operations;
                        break;

                    case SortType.CountingSort:
                        switch (fileArray.GetType().Name)
                        {
                            case "FileArray":
                                var arraySorter = new CountingSorter<FileArray>("sorted.txt");
                                arraySorter.CountSort((FileArray)sortedArray, maxValue);
                                operations = arraySorter.operations;
                                break;
                            case "FileLinkedList":
                                var linkeListSorter = new CountingSorter<FileLinkedList>("sorted.txt");
                                linkeListSorter.CountSort((FileLinkedList)sortedArray, maxValue);
                                operations = linkeListSorter.operations;
                                break;
                        }
                        break;

                    case SortType.MergeSort:
                        switch (fileArray.GetType().Name)
                        {
                            case "FileArray":
                                var arraySorter = new MergeSorter<FileArray>("sorted.txt");
                                sortedArray = arraySorter.MergeSort((FileArray)fileArray);
                                operations = arraySorter.operations;
                                break;
                            case "FileLinkedList":
                                var linkeListSorter = new MergeSorter<FileLinkedList>("sorted.txt");
                                sortedArray = linkeListSorter.MergeSort((FileLinkedList)fileArray);
                                operations = linkeListSorter.operations;
                                break;
                        }
                        break;


                }

                if (output)
                {
                    printArray(sortedArray);
                }
                Console.WriteLine("Time Taken: " + (DateTime.Now - startTime).TotalMilliseconds);
                Console.WriteLine("operations: " + operations);


            }
            Console.WriteLine("Start From Beginning? (Y/N)");
            if (String.Compare(Console.ReadLine(), "Y") == 0)
            {
                Main(new string[0]);
            }
        }
        static IArray CreateArray(ArrayType arrayType, string fileName)
        {
            switch (arrayType)
            {
                case ArrayType.LinkedList:
                    return new FileLinkedList(fileName);
                    break;
                case ArrayType.FileArray:
                    return new FileArray(fileName);
                    break; 
                default:
                    return  new FileArray(fileName);
            }
        }
        static void generateArrayFile(string fileName, int size)
        {
            using (var MyFile = System.IO.File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var generator = new Random(DateTime.Now.Millisecond);
                MyFile.Write(BitConverter.GetBytes(size), 0, bufSize);
                for (var i = 0; i < size; i++)
                {
                    var nextNum = generator.Next(0, 1000);
                    MyFile.Write(BitConverter.GetBytes(nextNum), 0, bufSize);
                }

            }
        }
        static void generateLinkedListFile(string fileName, int size)
        {
            using (var MyFile = System.IO.File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var generator = new Random(DateTime.Now.Millisecond);
                MyFile.Write(BitConverter.GetBytes(size), 0, bufSize);
                MyFile.Write(BitConverter.GetBytes(bufSize * 2), 0, bufSize);
                for (var i = 0; i < size; i++)
                {
                    var nextNum = generator.Next(0, 1000);
                    MyFile.Write(BitConverter.GetBytes(nextNum), 0, bufSize);
                    if (i + 1 < size)
                    {
                        MyFile.Write(BitConverter.GetBytes(((i + 2) * bufSize * 2)), 0, bufSize);
                    }
                    else
                    {
                        MyFile.Write(BitConverter.GetBytes(0), 0, bufSize);

                    }
                }

            }
        }
        static void printArray(IArray fileArray)
        {
            fileArray.MoveToStart();
            Console.WriteLine("--------");
            Console.WriteLine("Size: " + fileArray.Count());
            while (true)
            {
                try
                {
                    Console.WriteLine(fileArray.ReadNext());
                }
                catch (EndOfStreamException ex)
                {
                    break;
                }
            }
            Console.WriteLine("--------");
        }
    }
}
