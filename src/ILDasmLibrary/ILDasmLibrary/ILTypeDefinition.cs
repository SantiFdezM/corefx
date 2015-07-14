using ILDasmLibrary.Decoder;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System;
using System.Reflection;
using ILDasmLibrary.Visitor;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing a type definition within an assembly.
    /// </summary>
    public struct ILTypeDefinition : IVisitable
    {
        private Readers _readers;
        private readonly TypeDefinition _typeDefinition;
        private string _name;
        private string _fullName;
        private string _namespace;
        private IEnumerable<ILMethodDefinition> _methodDefinitions;
        private Dictionary<int, int> _methodImplementationDictionary;
        private int _token;
        private IEnumerable<string> _genericParameters;
        private IEnumerable<ILField> _fieldDefinitions;
        private IEnumerable<ILTypeDefinition> _nestedTypes;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private string _baseType;

        internal ILTypeDefinition(TypeDefinition typeDef, ref Readers readers, int token)
        {
            _typeDefinition = typeDef;
            _token = token;
            _readers = readers;
            _name = null;
            _fullName = null;
            _namespace = null;
            _methodDefinitions = null;
            _methodDefinitions = null;
            _genericParameters = null;
            _fieldDefinitions = null;
            _nestedTypes = null;
            _customAttributes = null;
            _baseType = null;
            _methodImplementationDictionary = null;
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
                    _fullName = SignatureDecoder.DecodeType(MetadataTokens.TypeDefinitionHandle(_token), _readers.Provider).ToString(false);
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
                return ILDecoder.GetCachedValue(_typeDefinition.Name, _readers, ref _name);
            }
        }
        
        /// <summary>
        /// Property containing the namespace name. 
        /// </summary>
        public string Namespace
        {
            get
            {
                return ILDecoder.GetCachedValue(_typeDefinition.Namespace, _readers ,ref _namespace);
            }
        }

        public bool IsGeneric
        {
            get
            {
                return GenericParameters.Count() != 0;
            }
        }

        public bool IsNested
        {
            get
            {
                return !_typeDefinition.GetDeclaringType().IsNil;
            }
        }

        public bool IsInterface
        {
            get
            {
                return _typeDefinition.BaseType.IsNil;
            }
        }

        public string BaseType
        {
            get
            {
                if (IsInterface) return null;
                if(_baseType == null)
                {
                    _baseType = SignatureDecoder.DecodeType(_typeDefinition.BaseType, _readers.Provider).ToString(false);
                }
                return _baseType;
            }
        }

        public TypeAttributes Attributes
        {
            get
            {
                return _typeDefinition.Attributes;
            }
        }

        public IEnumerable<InterfaceImplementation> InterfaceImplementations
        {
            get
            {
                throw new NotImplementedException("not implemented Interface Impl on Type Def");
            }
        }
        
        public IEnumerable<EventDefinition> Events
        {
            get
            {
                throw new NotImplementedException("Not implemented Events on typedef");
            }
        }

        public IEnumerable<PropertyDefinition> Properties
        {
            get
            {
                throw new NotImplementedException("Not implemented properties on type definition");
            }
        }

        public IEnumerable<ILTypeDefinition> NestedTypes
        {
            get
            {
                if(_nestedTypes == null)
                {
                    _nestedTypes = GetNestedTypes();
                }
                return _nestedTypes;
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
        public IEnumerable<ILMethodDefinition> MethodDefinitions
        {
            get
            {
                if (_methodDefinitions == null)
                {
                    _methodDefinitions = GetMethodDefinitions();
                }
                return _methodDefinitions;
            }
        }

        public IEnumerable<ILField> FieldDefinitions
        {
            get
            {
                if (_fieldDefinitions == null)
                {
                    _fieldDefinitions = GetFieldDefinitions();
                }
                return _fieldDefinitions;
            }
        }

        public IEnumerable<ILCustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttributes == null)
                {
                    _customAttributes = GetCustomAttributes();
                }
                return _customAttributes;
            }
        }

        /// <summary>
        /// Method that returns the token of a method declaration given the method token that overrides it. Returns 0 if the token doesn't represent an overriden method.
        /// </summary>
        /// <param name="methodBodyToken">Token of the method body that overrides a declaration.</param>
        /// <returns>token of the method declaration, 0 if there is no overriding of that method.</returns>
        public int GetOverridenMethodToken(int methodBodyToken)
        {
            int result = 0;
            MethodImplementationDictionary.TryGetValue(methodBodyToken, out result);
            return result;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        #endregion

        #region Private Methods
        private IEnumerable<ILMethodDefinition> GetMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            foreach (var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                yield return ILMethodDefinition.Create(method, MetadataTokens.GetToken(handle), ref _readers, this);
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

        private IEnumerable<ILField> GetFieldDefinitions()
        {
            foreach(var handle in _typeDefinition.GetFields())
            {
                var field = _readers.MdReader.GetFieldDefinition(handle);
                yield return new ILField(field, _readers.MdReader);
            }
        }

        private IEnumerable<ILTypeDefinition> GetNestedTypes()
        {
            foreach(var handle in _typeDefinition.GetNestedTypes())
            {
                if (handle.IsNil)
                {
                    continue;
                }
                var typeDefinition = _readers.MdReader.GetTypeDefinition(handle);
                yield return new ILTypeDefinition(typeDefinition, ref _readers, MetadataTokens.GetToken(handle));
            }
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _typeDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
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

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        #endregion
    }
}
