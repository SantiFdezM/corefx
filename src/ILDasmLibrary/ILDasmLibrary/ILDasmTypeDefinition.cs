using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public class ILDasmTypeDefinition : ILDasmReaders
    {
        private TypeDefinition _typeDefinition;
        private string _name;
        private string _namespace;

        internal ILDasmTypeDefinition(TypeDefinition typeDef, Readers readers)
            : base(readers)
        {
            _typeDefinition = typeDef;
        }

        public string Name
        {
            get
            {
                if(_name == null)
                {
                    _name = _readers.MdReader.GetString(_typeDefinition.Name);
                }
                return _name;
            }
        }

        public string Namespace
        {
            get
            {
                if(_namespace == null)
                {
                    _namespace = _readers.MdReader.GetString(_typeDefinition.Namespace);
                }
                return _namespace;
            }
        }
    }
}
