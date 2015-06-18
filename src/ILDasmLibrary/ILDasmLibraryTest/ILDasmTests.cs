using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using ILDasmLibrary;

namespace ILDasmLibraryTest
{
    [TestClass]
    public class ILDasmTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                if (!File.Exists(@"C:\Users\t-safer\Desktop\Class1.ilexe"))
                {
                    Debug.WriteLine("Doesn't exist");
                    return;
                }
                var ildasm = new ILDasm(@"C:\Users\t-safer\Desktop\Class1.ilexe");
                var types = ildasm.Assembly.TypeDefinitions;
                Debug.WriteLine(ildasm.Assembly.PublicKey);
                int i = 0;
                foreach (var type in types)
                {
                    var methods = type.MethodDefinitions;
                    foreach (var method in methods)
                    {
                        string me = method.DumpMethod(true);
                        Console.WriteLine("parsed");
                        i++;
                    }
                }
                Console.WriteLine("Methods parsed: " + i);
                watch.Stop();
                Console.WriteLine("Time: " + watch.Elapsed);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }
            Assert.IsTrue(true);
        }
    }
}
