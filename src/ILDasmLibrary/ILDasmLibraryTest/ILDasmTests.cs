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
            Stopwatch watch = new Stopwatch();
            int i = 0;
            try
            {
                string path = "Assemblies/Class1.ilexe";
                if (!File.Exists(path))
                {
                    Assert.Fail("File not found");
                    return;
                }
                var ildasm = new ILDasm(path);
                var types = ildasm.Assembly.TypeDefinitions;
                watch.Start();
                using (StreamWriter file = new StreamWriter("../../Output/foo.il"))
                {
                    foreach (var type in types)
                    {
                        var methods = type.MethodDefinitions;
                        foreach (var method in methods)
                        {
                            string me = method.DumpMethod(true);
                            file.WriteLine(me);
                            i++;
                        }
                    }
                    watch.Stop();
                    file.WriteLine("Time elapsed: " + watch.Elapsed);
                    file.WriteLine("Methods Parsed: " + i);
                }
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
