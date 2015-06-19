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
                string path = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll";
                if (!File.Exists(path))
                {
                    Assert.Fail("File not found");
                    return;
                }
                var ildasm = new ILDasm(path);
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
