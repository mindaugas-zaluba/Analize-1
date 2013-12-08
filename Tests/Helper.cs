using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analize.Array;

namespace Tests
{
    class Helper
    {
        static public FileArray GenerateFileArray()
        {
            var sampleFile = new FileArray("smaple.txt");
            sampleFile.Push(2);
            sampleFile.Push(5);
            sampleFile.Push(8);
            sampleFile.Push(9);
            sampleFile.Push(7);
            sampleFile.Push(10);
            sampleFile.Push(3);
            sampleFile.Push(7);
            sampleFile.Push(1);
            sampleFile.Push(0);
            return sampleFile;
        }
    }
}
