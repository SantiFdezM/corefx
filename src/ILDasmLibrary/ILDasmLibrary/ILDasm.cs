using System.IO;
using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class that represents an assembly file, containing it's assembly definition to access types and methods.
    /// </summary>
    public class ILDasm
    {
        private readonly Readers _readers;
        private readonly ILDasmAssembly _assembly;

        /// <summary>
        /// Constructor that receives a file stream to create the assembly out from it's file image.
        /// </summary>
        /// <param name="fileStream"></param>
        public ILDasm(Stream fileStream)
        {
            _readers = Readers.Create(fileStream);
            AssemblyDefinition assemblyDef = _readers.MdReader.GetAssemblyDefinition();
            _assembly = new ILDasmAssembly(assemblyDef, _readers);
        }

        /// <summary>
        /// Constructor that takes the file path.
        /// </summary>
        /// <param name="path"></param>
        public ILDasm(string path)
            : this(File.OpenRead(path))
        {
        }

        /// <summary>
        /// Property that contains the assembly definition from the image file.
        /// </summary>
        public ILDasmAssembly Assembly
        {
            get
            {
                return _assembly;
            }
        }
    }
    
}
