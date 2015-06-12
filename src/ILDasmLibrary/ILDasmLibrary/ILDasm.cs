using System.IO;
using System.Reflection.Metadata;

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
