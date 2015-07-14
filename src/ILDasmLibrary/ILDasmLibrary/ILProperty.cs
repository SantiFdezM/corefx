using ILDasmLibrary.Decoder;
using ILDasmLibrary.Visitor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;

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
        private bool _isSignatureInitialized;

        internal static ILProperty Create(PropertyDefinition propertyDef, ref Readers readers)
        {
            ILProperty property = new ILProperty();
            property._propertyDef = propertyDef;
            property._readers = readers;
            property._isSignatureInitialized = false;
            return property;
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_propertyDef.Name, _readers, ref _name);
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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _propertyDef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }

    }
}
