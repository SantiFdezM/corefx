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

        internal ILDasmTypeDefinition(TypeDefinition typeDef, Readers readers)
            : base(readers)
        {
            _typeDefinition = typeDef;
        }

        public string Name
        {
            get
            {
                return _readers.MdReader.GetString(_typeDefinition.Name);
            }
        }

        public string Namespace
        {
            get
            {
                return _readers.MdReader.GetString(_typeDefinition.Namespace);
            }
        }
    }
}
