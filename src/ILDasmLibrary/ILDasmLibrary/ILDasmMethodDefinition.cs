using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    public class ILDasmMethodDefinition : ILDasmReaders
    {
        private MethodDefinition _methodDefinition;
        private string _name;
        private int _rva;

        internal ILDasmMethodDefinition(MethodDefinition methodDefinition, Readers readers) 
            : base(readers)
        {
            _methodDefinition = methodDefinition;
            _rva = -1;
        }

        public string Name
        {
            get
            {
                if(_name == null)
                {
                    _name = _readers.MdReader.GetString(_methodDefinition.Name);
                }
                return _name;
            }
        }

        public int RelativeVirtualAdress
        {
            get
            {
                if(_rva == -1)
                {
                    _rva = _methodDefinition.RelativeVirtualAddress;
                }
                return _rva;
            }
        }

        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAdress);
        }
    }
}