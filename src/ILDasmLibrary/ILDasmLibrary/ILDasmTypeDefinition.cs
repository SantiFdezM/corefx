using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILDasmLibrary
{
    public class ILDasmTypeDefinition : ILDasmObject
    {
        private TypeDefinition _typeDefinition;
        private string _name;
        private string _namespace;
        private IList<ILDasmMethodDefinition> _methodDefinitions;
        private Dictionary<int, int> _methodImplementationDictionary;

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

        internal Dictionary<int, int> MethodImplementationDictionary
        {
            get
            {
                if(_methodImplementationDictionary == null)
                {
                   PopulateMethodImplementationDictionary();
                }
                return _methodImplementationDictionary;
            }
        }

        public IEnumerable<ILDasmMethodDefinition> MethodDefinitions
        {
            get
            {
                if (_methodDefinitions == null)
                {
                    PopulateMethodDefinitions();
                }
                return _methodDefinitions.AsEnumerable<ILDasmMethodDefinition>();
            }
        }

        public int GetMethodDeclTokenFromImplementation(int token)
        {
            int result = 0;
            MethodImplementationDictionary.TryGetValue(token, out result);
            return result;
        }

        private void PopulateMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            _methodDefinitions = new List<ILDasmMethodDefinition>();
            foreach(var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                _methodDefinitions.Add(new ILDasmMethodDefinition(method,MetadataTokens.GetToken(handle),_readers, this));
            }
        }

        private void PopulateMethodImplementationDictionary()
        {
            var implementations = _typeDefinition.GetMethodImplementations();
            Dictionary<int, int> dictionary = new Dictionary<int, int>(implementations.Count);
            foreach(var implementationHandle in implementations)
            {
                var implementation = _readers.MdReader.GetMethodImplementation(implementationHandle);
                int declarationToken = MetadataTokens.GetToken(implementation.MethodDeclaration);
                int bodyToken = MetadataTokens.GetToken(implementation.MethodBody);
                dictionary.Add(bodyToken, declarationToken);
            }
            _methodImplementationDictionary = dictionary;
        }
    }
}
