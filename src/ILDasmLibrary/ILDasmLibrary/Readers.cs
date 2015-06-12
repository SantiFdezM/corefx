using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace ILDasmLibrary
{
    internal class Readers
    {
        private readonly PEReader _peReader;
        private readonly MetadataReader _mdReader;

        public PEReader PEReader { get { return _peReader; } }
        public MetadataReader MdReader { get { return _mdReader; } }

        private Readers(Stream fileStream)
        {
            _peReader = new PEReader(fileStream);
            _mdReader = _peReader.GetMetadataReader();
        }

        public static Readers Create(Stream fileStream)
        {
            return new Readers(fileStream);
        }
    }
}
