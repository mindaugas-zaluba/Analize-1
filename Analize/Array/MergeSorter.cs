using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analize.Array
{

        public class MergeSorter<T> where T: IArray, new()
        {
            public string fileName;
            public int operations = 0;
            public MergeSorter(string fileName)
            {
                this.fileName = fileName;
                operations = 0;
            }

            public T MergeSort(T arrIntegers)
            {        
                if (arrIntegers.Count() == 1)
                {
                    operations++;

                    return arrIntegers;
                }

                IArray arrSortedInt = new T().Initialize(Guid.NewGuid().ToString());
                int middle = (int)arrIntegers.Count() / 2;
                IArray leftArray = arrIntegers.GetRange(0, middle, Guid.NewGuid().ToString());
                IArray rightArray = arrIntegers.GetRange(middle, arrIntegers.Count() - middle, Guid.NewGuid().ToString());
                leftArray = MergeSort((T)leftArray);
                rightArray = MergeSort((T)rightArray);
                int leftptr = 0;
                int rightptr = 0;

                
                operations += 9;

                for (int i = 0; i < leftArray.Count() + rightArray.Count(); i++)
                {
                    if (leftptr == leftArray.Count())
                    {
                        arrSortedInt.Push(rightArray.ReadAt(rightptr));
                        rightptr++;
                        //First If
                        operations += 3;
                    }
                    else if (rightptr == rightArray.Count())
                    {
                        arrSortedInt.Push(leftArray.ReadAt(leftptr));
                        leftptr++;
                        //Second If
                        operations += 4;
                    }
                    else if (leftArray.ReadAt(leftptr) < rightArray.ReadAt(rightptr))
                    {
                       
                        arrSortedInt.Push(leftArray.ReadAt(leftptr));
                        leftptr++;
                        //Third If
                        operations += 5;
                    }
                    else
                    {
                        arrSortedInt.Push(rightArray.ReadAt(rightptr));
                        rightptr++;
                        //Fouth If
                        operations += 6;
                    }
                    operations++;
                }

                leftArray.SelfDestruct();
                rightArray.SelfDestruct();
                arrIntegers.SelfDestruct();
                return (T)arrSortedInt;
            }

        }
    
}
