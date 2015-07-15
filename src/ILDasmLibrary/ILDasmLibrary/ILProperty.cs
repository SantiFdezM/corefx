using ILDasmLibrary.Decoder;
using ILDasmLibrary.Visitor;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Text;
using System;

namespace ILDasmLibrary
{
    public struct ILProperty : IVisitable
    {
        private PropertyDefinition _propertyDef;
        private Readers _readers;
        private string _name;
        private MethodSignature<ILType> _signature;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private ILMethodDefinition _getter;
        private ILMethodDefinition _setter;
        private bool _isGetterInitialized;
        private bool _isSetterInitialized;
        private PropertyAccessors _accessors;
        private bool _isSignatureInitialized;
        private ILTypeDefinition _typeDefinition;
        private int _token;

        internal static ILProperty Create(PropertyDefinition propertyDef, int token, ref Readers readers, ILTypeDefinition typeDefinition)
        {
            ILProperty property = new ILProperty();
            property._typeDefinition = typeDefinition;
            property._propertyDef = propertyDef;
            property._readers = readers;
            property._isSignatureInitialized = false;
            property._isGetterInitialized = false;
            property._isSetterInitialized = false;
            property._token = token;
            property._accessors = propertyDef.GetAccessors();
            return property;
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        public ILTypeDefinition DeclaringType
        {
            get
            {
                return _typeDefinition;
            }
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_propertyDef.Name, _readers, ref _name);
            }
        }

        public bool HasGetter
        {
            get
            {
                return !_accessors.Getter.IsNil;
            }
        }

        public bool HasSetter
        {
            get
            {
                return !_accessors.Setter.IsNil;
            }
        }

        public bool HasDefault
        {
            get
            {
                return Attributes.HasFlag(PropertyAttributes.HasDefault);
            }
        }

        public ILMethodDefinition Getter
        {
            get
            {
                if (!_isGetterInitialized)
                {
                    _isGetterInitialized = true;
                    if (HasGetter)
                    {
                        _getter = ILMethodDefinition.Create(_accessors.Getter, ref _readers, _typeDefinition);
                    }
                }
                return _getter;
            }
        }

        public ILMethodDefinition Setter
        {
            get
            {
                if (!_isSetterInitialized)
                {
                    _isSetterInitialized = true;
                    if (HasSetter)
                    {
                        _setter = ILMethodDefinition.Create(_accessors.Setter, ref _readers, _typeDefinition);
                    }
                }
                return _setter;
            }
        }

        public MethodSignature<ILType> Signature
        {
            get
            {
                if (!_isSignatureInitialized)
                {
                    _isSignatureInitialized = true;
                    _signature = SignatureDecoder.DecodeMethodSignature(_propertyDef.Signature, _readers.Provider);
                }
                return _signature;
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

        public PropertyAttributes Attributes
        {
            get
            {
                return _propertyDef.Attributes;
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetDecodedSignature()
        {
            string attributes = GetAttributesForSignature();
            StringBuilder signature = new StringBuilder();
            if (Signature.Header.IsInstance)
            {
                signature.Append("instance ");
            }
            signature.Append(Signature.ReturnType);
            return string.Format("{0}{1} {2}{3}", attributes, signature.ToString(), Name, ILDecoder.DecodeSignatureParamerTypes(Signature));
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _propertyDef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }

        private string GetAttributesForSignature()
        {
            if (Attributes.HasFlag(PropertyAttributes.SpecialName))
            {
                return "specialname ";
            }
            if (Attributes.HasFlag(PropertyAttributes.RTSpecialName))
            {
                return "rtspecialname ";
            }
            return string.Empty;
        }

    }
}
