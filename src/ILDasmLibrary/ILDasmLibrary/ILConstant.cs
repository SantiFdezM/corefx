using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public struct ILConstant
    {
        private Readers _readers;
        private Constant _constant;
        private ConstantTypeCode _typeCode;
        private bool _isTypeInitialized;
        private object _value;

        internal ILConstant Create(Constant constant, ref Readers readers)
        {
            ILConstant ilConstant = new ILConstant();
            ilConstant._constant = constant;
            ilConstant._readers = readers;
            ilConstant._isTypeInitialized = false;
            return ilConstant;
        }

        public ConstantTypeCode TypeCode
        {
            get
            {
                if (!_isTypeInitialized)
                {
                    _isTypeInitialized = true;
                    _typeCode = _constant.TypeCode;
                }
                return _typeCode;
            }
        }

        public object Value
        {
            get
            {
                if(_value == null)
                {
                    _value = GetValue();
                }
                return _value;
            }
        }

        public string GetValueString()
        {
            BlobReader reader = _readers.MdReader.GetBlobReader(_constant.Value);

            switch (TypeCode)
            {
                case ConstantTypeCode.Byte:
                    return string.Format("uint8({0})", reader.ReadByte().ToString("X2"));
                case ConstantTypeCode.Boolean:
                    return string.Format("bool({0})", reader.ReadBoolean().ToString());
                case ConstantTypeCode.Char:
                    return string.Format("char({0})", reader.ReadChar());
                case ConstantTypeCode.SByte:
                    return string.Format("int8({0})", reader.ReadSByte());
                case ConstantTypeCode.Int16:
                    return string.Format("int16({0})", reader.ReadInt16().ToString());
                case ConstantTypeCode.Int32:
                    return string.Format("int32({0})", reader.ReadInt32().ToString());
                case ConstantTypeCode.Int64:
                    return string.Format("int64({0})", reader.ReadInt64().ToString());
                case ConstantTypeCode.Single:
                    return GetFloatString(reader.ReadSingle());
                case ConstantTypeCode.Double:
                    return GetDoubleString(reader.ReadDouble());
                case ConstantTypeCode.String:
                    return string.Format("char*(\"{0}\")",reader.ReadSerializedString());
                case ConstantTypeCode.UInt16:
                    return string.Format("uint16({0})", reader.ReadUInt16().ToString());
                case ConstantTypeCode.UInt32:
                    return string.Format("uint32({0})", reader.ReadUInt32().ToString());
                case ConstantTypeCode.UInt64:
                    return string.Format("uint64({0})", reader.ReadUInt64().ToString());
                case ConstantTypeCode.NullReference:
                    return "nullref";
                default:
                    throw new BadImageFormatException("Invalid Constant Type Code");
            }

        }

        private string GetFloatString(float single)
        {
            if (float.IsNaN(single) || float.IsInfinity(single))
            {
                var data = BitConverter.GetBytes(single);
                StringBuilder sb = new StringBuilder();
                sb.Append("0x");
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    sb.Append(data[i]);
                }
                return string.Format("float32({0})", sb.ToString());
            }
            return string.Format("float32({0})", single.ToString());
        }

        private string GetDoubleString(double number)
        {
            if (double.IsNaN(number) || double.IsInfinity(number))
            {
                var data = BitConverter.GetBytes(number);
                StringBuilder sb = new StringBuilder();
                sb.Append("0x");
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    sb.Append(data[i]);
                }
                return string.Format("float64({0})", sb.ToString());
            }
            return string.Format("float64({0})", number.ToString());
        }

        private object GetValue()
        {
            BlobReader reader = _readers.MdReader.GetBlobReader(_constant.Value);

            switch (TypeCode)
            {
                case ConstantTypeCode.Byte:
                    return reader.ReadByte();
                case ConstantTypeCode.Boolean:
                    return reader.ReadBoolean();
                case ConstantTypeCode.Char:
                    return reader.ReadChar();
                case ConstantTypeCode.SByte:
                    return reader.ReadSByte();
                case ConstantTypeCode.Int16:
                    return reader.ReadInt16();
                case ConstantTypeCode.Int32:
                    return reader.ReadInt32();
                case ConstantTypeCode.Int64:
                    return reader.ReadInt64();
                case ConstantTypeCode.Single:
                    return reader.ReadSingle();
                case ConstantTypeCode.Double:
                    return reader.ReadDouble();
                case ConstantTypeCode.String:
                    return reader.ReadSerializedString();
                case ConstantTypeCode.UInt16:
                    return reader.ReadUInt16();
                case ConstantTypeCode.UInt32:
                    return reader.ReadUInt32();
                case ConstantTypeCode.UInt64:
                    return reader.ReadUInt64();
                case ConstantTypeCode.NullReference:
                    return null;
                default:
                    throw new BadImageFormatException("Invalid Constant Type Code");
            }
        }

    }
}
