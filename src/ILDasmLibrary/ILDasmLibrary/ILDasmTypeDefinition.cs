using ILDasmLibrary.Decoder;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing a type definition within an assembly.
    /// </summary>
    public class ILDasmTypeDefinition : ILDasmObject
    {
        private readonly TypeDefinition _typeDefinition;
        private string _name;
        private string _fullName;
        private string _namespace;
        private IList<ILDasmMethodDefinition> _methodDefinitions;
        private Dictionary<int, int> _methodImplementationDictionary;
        private int _token;
        private IEnumerable<string> _genericParameters;

        internal ILDasmTypeDefinition(TypeDefinition typeDef, Readers readers, int token)
            : base(readers)
        {
            _typeDefinition = typeDef;
            _token = token;
        }

        #region Public APIs

        /// <summary>
        /// Type full name
        /// </summary>
        public string FullName
        {
            get
            {
                if(_fullName == null)
                {
                    _fullName = SignatureDecoder.DecodeType(MetadataTokens.TypeDefinitionHandle(_token), new ILDasmTypeProvider(_readers.MdReader)).ToString(false);
                }
                return _fullName;
            }
        }

        /// <summary>
        /// Property that contains the type name. 
        /// </summary>
        public string Name
        {
            get
            {
                return GetCachedValue(_typeDefinition.Name, ref _name);
            }
        }
        
        /// <summary>
        /// Property containing the namespace name. 
        /// </summary>
        public string Namespace
        {
            get
            {
                return GetCachedValue(_typeDefinition.Namespace, ref _namespace);
            }
        }

        /// <summary>
        /// Type token.
        /// </summary>
        public int Token
        {
            get
            {
               return _token;
            }
        }

        public bool IsGeneric
        {
            get
            {
                return GenericParameters.Count() != 0;
            }
        }

        public IEnumerable<string> GenericParameters
        {
            get
            {
                if(_genericParameters == null)
                {
                    _genericParameters = GetGenericParameters();
                }
                return _genericParameters;
            }
        }
        
        /// <summary>
        /// Property containing all the method definitions within a type. 
        /// </summary>
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

        /// <summary>
        /// Method that returns the token of a method declaration given the method token that overrides it. Returns 0 if the token doesn't represent an overriden method.
        /// </summary>
        /// <param name="token">Token of the method body that overrides a declaration.</param>
        /// <returns>token of the method declaration, 0 if there is no overriding of that method.</returns>
        public int GetMethodDeclTokenFromImplementation(int token)
        {
            int result = 0;
            MethodImplementationDictionary.TryGetValue(token, out result);
            return result;
        }

        public string Dump(bool showBytes = false)
        {
            return new ILDasmWriter(indentation: 0).DumpType(this, showBytes);
        }
        #endregion

        #region Private Methods
        private void PopulateMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            _methodDefinitions = new List<ILDasmMethodDefinition>();
            foreach (var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                _methodDefinitions.Add(new ILDasmMethodDefinition(method, MetadataTokens.GetToken(handle), _readers, this));
            }
        }

        private void PopulateMethodImplementationDictionary()
        {
            var implementations = _typeDefinition.GetMethodImplementations();
            Dictionary<int, int> dictionary = new Dictionary<int, int>(implementations.Count);
            foreach (var implementationHandle in implementations)
            {
                var implementation = _readers.MdReader.GetMethodImplementation(implementationHandle);
                int declarationToken = MetadataTokens.GetToken(implementation.MethodDeclaration);
                int bodyToken = MetadataTokens.GetToken(implementation.MethodBody);
                dictionary.Add(bodyToken, declarationToken);
            }
            _methodImplementationDictionary = dictionary;
        }

        private IEnumerable<string> GetGenericParameters()
        {
            foreach(var handle in _typeDefinition.GetGenericParameters())
            {
                var parameter = _readers.MdReader.GetGenericParameter(handle);
                yield return _readers.MdReader.GetString(parameter.Name);
            }
        }

        #endregion

        #region Internal Members

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

        #endregion
    }
}
