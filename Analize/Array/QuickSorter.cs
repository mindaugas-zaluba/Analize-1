using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analize.Array
{
    class QuickSorter
    {
        public int operations = 0;

        public QuickSorter()
            {

                operations = 0;
            }

        public void Quicksort(IArray elements, int left, int right)
        {
            int i = left;
            int j = right;
            int pivot = elements.ReadAt((left + right) / 2);

            operations += 4;

            while (i <= j)
            {
                while (elements.ReadAt(i).CompareTo(pivot) < 0)
                {
                    i++;
                    operations +=2;
                }

                while (elements.ReadAt(j).CompareTo(pivot) > 0)
                {
                    j--;
                    operations+=2;
                }

                if (i <= j)
                {
                    // Swap
                    int tmp = elements.ReadAt(i);
                    elements.ReplaceAt(i,elements.ReadAt(j)); 
                    elements.ReplaceAt(j, tmp);

                    i++;
                    j--;

                    operations += 7;
                }
                //while check
                operations += 1;
            }
            

            // Recursive calls
            if (left < j)
            {
                Quicksort(elements, left, j);
                operations++;
            }

            if (i < right)
            {
                Quicksort(elements, i, right);
                operations++;

            }

            //two check operations
            operations += 2;
        }
    }
}
