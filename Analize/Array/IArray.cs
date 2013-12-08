using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analize.Array
{
    public interface IArray :IDisposable
    {
        IArray GetRange(int start, int length, string fileName);
        IArray Initialize(string fileName);
        int ReadAt(long index);
        void ReplaceAt(long index, int value);
        void MoveToStart();
        int ReadNext();
        int Count();
        void InsertAt(int index, int value);
        void RemoveAt(int index);
        void Push(int val);
        void SelfDestruct();

    }
}
