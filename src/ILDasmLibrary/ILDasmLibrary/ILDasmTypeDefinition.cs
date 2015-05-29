using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public class ILDasmTypeDefinition
    {
        private TypeDefinition _typeDefinition;

        internal ILDasmTypeDefinition(TypeDefinition typeDef)
        {
            _typeDefinition = typeDef;
        }

        public string Name
        {
            get
            {
                return Readers.MdReader.GetString(_typeDefinition.Name);
            }
        }

        public string Namespace
        {
            get
            {
                return Readers.MdReader.GetString(_typeDefinition.Namespace);
            }
        }
    }
}
