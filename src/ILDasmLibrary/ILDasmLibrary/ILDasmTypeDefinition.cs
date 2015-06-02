using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public class ILDasmTypeDefinition : ILDasmObject
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
                return GetCachedValue(_typeDefinition.Name, ref _name);
            }
        }

        public string Namespace
        {
            get
            {
                return GetCachedValue(_typeDefinition.Namespace, ref _namespace);
            }
        }

        public IEnumerable<ILDasmMethodDefinition> MethodDefinitions
        {
            get
            {
                if (_methodDefinitions == null)
                {
                    GetMethodDefinitions();
                }
                return _methodDefinitions.AsEnumerable<ILDasmMethodDefinition>();
            }
        }

        private void GetMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            _methodDefinitions = new Collection<ILDasmMethodDefinition>();
            foreach(var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                _methodDefinitions.Add(new ILDasmMethodDefinition(method, _readers));
            }
        }
    }
}
