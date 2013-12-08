using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analize.Array
{
    class CountingSorter<T> where T : IArray, new()
    {

        public int operations = 0;
        public string fileName;
        public CountingSorter(string fileName)
        {
            this.fileName = fileName;
            operations = 0;
        }


        public void CountSort(T elements, int maxValue)
        {

            IArray countArray = new T().Initialize(Guid.NewGuid().ToString());
            operations++;

            for (int i = 0; i < maxValue + 1; i++) countArray.Push(0);
            operations += maxValue;

            for (int i = 0; i < elements.Count(); i++)
            {
                var x = elements.ReadAt(i);
                var y = countArray.ReadAt(x);
                countArray.ReplaceAt(x, y + 1);

                operations += 3;
            }
            operations += elements.Count();

            int index = 0;
            operations ++;

            for (int i = 0; i < countArray.Count(); i++)
            {
                var n = countArray.ReadAt(i);
                operations++;

                while (n != 0)
                {
                    elements.ReplaceAt(index++, i);
                    countArray.ReadAt(i);
                    n--;
                    operations += 4;
                }

            }
            operations += countArray.Count();
            countArray.SelfDestruct();
        }
    }
}
