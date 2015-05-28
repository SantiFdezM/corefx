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
        public ILDasmAssembly Assembly; 
        public ILDasm(Stream fileStream)
        {
            Readers.PeReader = new PEReader(fileStream);
            BuildAssembly();
        }

        public ILDasm(string path)
            : this(File.OpenRead(path))
        {   
        }

        private void BuildAssembly()
        {
            AssemblyDefinition assemblyDef = Readers.MdReader.GetAssemblyDefinition();
            Assembly = new ILDasmAssembly(assemblyDef);
        }
    }

    internal static class Readers{
        private static PEReader _peReader;
        
        public static PEReader PeReader {
            get
            {
                return _peReader;
            }
            internal set
            {
                _peReader = value;
            }
        }

        public static MetadataReader MdReader
        {
            get
            {
                if(_peReader == null)
                {
                    throw new Exception("PEReader has to be initialized before trying to get MdReader");
                }
                return _peReader.GetMetadataReader();
            }
        }
   }
}
