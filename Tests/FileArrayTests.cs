using System;
using System.IO;
using Analize.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class FileArrayTests
    {
        [TestMethod]
        public void InitializationAndDestructionTest()
        {
            var testFile = new FileArray("test.txt");
            Assert.IsNotNull(testFile);
            testFile.SelfDestruct();
        }
        [TestMethod]
        public void ReadAtTest()
        {
            var testFile = Helper.GenerateFileArray();
            Assert.AreEqual(testFile.ReadAt(0), 2);
            Assert.AreEqual(testFile.ReadAt(2), 8);
            Assert.AreEqual(testFile.ReadAt(4), 7);
            Assert.AreEqual(testFile.ReadAt(5), 10);
            testFile.SelfDestruct();
        }
        [TestMethod]
        public void ReplaceAtTest()
        {
            var testFile = Helper.GenerateFileArray();
            testFile.ReplaceAt(0,11);
            Assert.AreEqual(testFile.ReadAt(0), 11);
            testFile.ReplaceAt(2, 12);
            Assert.AreEqual(testFile.ReadAt(2),12);
            testFile.ReplaceAt(4, 13);
            Assert.AreEqual(testFile.ReadAt(4), 13);
            testFile.ReplaceAt(5, 14);
            Assert.AreEqual(testFile.ReadAt(5), 14);
            testFile.SelfDestruct();
        }

        [TestMethod]
        public void PushTest()
        {
            var testFile = Helper.GenerateFileArray();
            testFile.Push(1);
            Assert.AreEqual(testFile.ReadAt(10), 1);
            testFile.Push(2);
            Assert.AreEqual(testFile.ReadAt(11), 2);
            testFile.SelfDestruct();

        }
        [TestMethod]
        public void RemoveAtTest()
        {
            using (var testFile = Helper.GenerateFileArray())
            {
                testFile.RemoveAt(0);
                Assert.AreEqual(testFile.ReadAt(0), 5);
                testFile.SelfDestruct();
            };
            using (var testFile = Helper.GenerateFileArray())
            {
                testFile.RemoveAt(6);
                Assert.AreEqual(testFile.ReadAt(6), 7);
                testFile.SelfDestruct();
            };
            using (var testFile = Helper.GenerateFileArray())
            {
                testFile.RemoveAt(9);
                testFile.Push(11);
                Assert.AreEqual(testFile.ReadAt(9), 11);
                testFile.SelfDestruct();
            };


        }
        [TestMethod]
        public void InsertAtTest()
        {
            using (var testFile = Helper.GenerateFileArray())
            {
               testFile.InsertAt(0,11);
               Assert.AreEqual(testFile.ReadAt(0), 11);
               Assert.AreEqual(testFile.ReadAt(1), 2);
               testFile.SelfDestruct(); 
            };
            using (var testFile = Helper.GenerateFileArray())
            {
               testFile.InsertAt(4,11);
               Assert.AreEqual(testFile.ReadAt(4), 11);
               Assert.AreEqual(testFile.ReadAt(3), 9);
               Assert.AreEqual(testFile.ReadAt(5), 7);
               testFile.SelfDestruct(); 
            };
            using (var testFile = Helper.GenerateFileArray())
            {
               testFile.InsertAt(9,11);
               Assert.AreEqual(testFile.ReadAt(9), 11);
               Assert.AreEqual(testFile.ReadAt(10), 0);
               testFile.SelfDestruct(); 
            };
            using (var testFile = Helper.GenerateFileArray())
            {
               testFile.InsertAt(10,11);
               Assert.AreEqual(testFile.ReadAt(10), 11);
               testFile.SelfDestruct(); 
            };
            

        }
    }
}
