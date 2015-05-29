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
        private ICollection<ILDasmMethodDefinition> _methodDefinitions;

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

        public IEnumerable<ILDasmMethodDefinition> MethodDefinitions
        {
            get
            {
                if (_methodDefinitions == null) GetMethodDefinitions();
                return _methodDefinitions.AsEnumerable<ILDasmMethodDefinition>();
            }
        }

        private void GetMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            foreach(var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                _methodDefinitions.Add(new ILDasmMethodDefinition(method, _readers));
            }
        }
    }
}
