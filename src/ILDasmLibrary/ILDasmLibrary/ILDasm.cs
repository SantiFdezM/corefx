using System;
using System.Collections.Generic;
using System.IO;
using System.IO.File;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public class ILDasm
    {
        private readonly Readers _readers;

        public ILDasmAssembly Assembly;
        public ILDasm(Stream fileStream)
        {
            _readers = Readers.Create(fileStream);
            AssemblyDefinition assemblyDef = _readers.MdReader.GetAssemblyDefinition();
            Assembly = new ILDasmAssembly(assemblyDef, _readers);
        }

        public ILDasm(string path)
            : this(File.OpenRead(path))
        {
        }
    }
    
}
