using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using ILDasmLibrary;

namespace ILDasmLibraryTest
{
    [TestClass]
    public class ILTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch watch = new Stopwatch();
            int i = 0;
            try
            {
                string path = "Assemblies/mscorlib.dll";
                if (!File.Exists(path))
                {
                    Assert.Fail("File not found");
                    return;
                }
                var assembly = ILAssembly.Create(path);
                var types = assembly.TypeDefinitions;
                watch.Start();
                using (StreamWriter file = new StreamWriter("../../Output/foo.il"))
                {
                    foreach (var type in types)
                    {
                        file.WriteLine(type.Dump(false));
                    }
                    watch.Stop();
                    file.WriteLine("Time elapsed: " + watch.Elapsed);
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
