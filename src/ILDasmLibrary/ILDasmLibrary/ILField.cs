using ILDasmLibrary.Decoder;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System;
using System.Text;
using ILDasmLibrary.Visitor;

namespace ILDasmLibrary
{
    public struct ILField : IVisitable
    {
        private readonly FieldDefinition _fieldDefinition;
        private readonly ILTypeDefinition _typeDefinition;
        private Readers _readers;
        private string _name;
        private string _type;
        private string _signature;
        private int _token;

        internal ILField(FieldDefinition fieldDefinition, int token, ref Readers readers, ILTypeDefinition typeDefinition)
        {
            _fieldDefinition = fieldDefinition;
            _token = token;
            _name = null;
            _type = null;
            _signature = null;
            _readers = readers;
            _typeDefinition = typeDefinition;
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_fieldDefinition.Name, _readers, ref _name);
            }
        }

        public string Type
        {
            get
            {
                if(_type == null)
                {
                    _type = SignatureDecoder.DecodeFieldSignature(_fieldDefinition.Signature, _readers.Provider).ToString();
                }
                return _type;
            }
        }

        public FieldAttributes Attributes
        {
            get
            {
                return _fieldDefinition.Attributes;
            }
        }

        public string Signature
        {
            get
            {
                if(_signature == null)
                {
                    _signature = GetSignature();
                }
                return _signature;
            }
        }

        public bool HasDefault
        {
            get
            {
                return Attributes.HasFlag(FieldAttributes.HasDefault);
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        private string GetSignature()
        {
            return string.Format("{0}{1} {2}{3} {4}", GetAccessibilityFlags(), GetContractFlags(), GetMarshalAttributes(), Type ,Name);
        }

        private string GetMarshalAttributes()
        {
            return string.Empty;
            // TO DO.
            //Need a Marshalling decoder because marshalling descriptor types have diferent specifications.
            //if (!Attributes.HasFlag(FieldAttributes.HasFieldMarshal))
            //    return string.Empty;
            //StringBuilder sb = new StringBuilder();
            //BlobReader reader = _readers.MdReader.GetBlobReader(_fieldDefinition.GetMarshallingDescriptor());
            //var type = SignatureDecoder.DecodeType(ref reader, _readers.Provider);
            //sb.Append("marshal(");
            //sb.Append(type);
            //sb.Append(") ");
            //return sb.ToString();
        }

        private string GetContractFlags()
        {
            StringBuilder sb = new StringBuilder();
            if (Attributes.HasFlag(FieldAttributes.Static))
            {
                sb.Append("static ");
            }
            if (Attributes.HasFlag(FieldAttributes.InitOnly))
            {
                sb.Append("initonly ");
            }
            if (Attributes.HasFlag(FieldAttributes.Literal))
            {
                sb.Append("literal ");
            }
            if (Attributes.HasFlag(FieldAttributes.NotSerialized))
            {
                sb.Append("notserialized ");
            }
            if (Attributes.HasFlag(FieldAttributes.RTSpecialName))
            {
                sb.Append("rtspecialname ");
            }
            if (Attributes.HasFlag(FieldAttributes.SpecialName))
            {
                sb.Append("specialname ");
            }
            if(sb.Length > 0)
                sb.Length--; //remove trailing space;
            return sb.ToString();
        }

        private string GetAccessibilityFlags()
        {
            if (Attributes.HasFlag(FieldAttributes.Public))
            {                      
                return "public ";  
            }                      
            if (Attributes.HasFlag(FieldAttributes.FamORAssem))
            {
                return "famorassem ";
            }
            if (Attributes.HasFlag(FieldAttributes.Family))
            {
                return "family ";
            }
            if (Attributes.HasFlag(FieldAttributes.Assembly))
            {
                return "assembly ";
            }
            if (Attributes.HasFlag(FieldAttributes.FamANDAssem))
            {
                return "famandassem ";
            }
            if (Attributes.HasFlag(FieldAttributes.Private))
            {
                return "private ";
            }
            return string.Empty;
        }
    }
}
