﻿using System;
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

        internal static ILConstant Create(Constant constant, ref Readers readers)
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
            object value = Value;
            switch (TypeCode)
            {
                case ConstantTypeCode.Byte:
                    return string.Format("uint8(0x{0})", ((byte)value).ToString("X2"));
                case ConstantTypeCode.Boolean:
                    return string.Format("bool({0})", ((bool)value).ToString());
                case ConstantTypeCode.Char:
                    return string.Format("char(0x{0})", ((int)(char)value).ToString("X4"));
                case ConstantTypeCode.SByte:
                    return string.Format("int8(0x{0})", ((sbyte)value).ToString("X2"));
                case ConstantTypeCode.Int16:
                    return string.Format("int16(0x{0})", ((short)value).ToString("X4"));
                case ConstantTypeCode.Int32:
                    return string.Format("int32(0x{0})", ((int)value).ToString("X8"));
                case ConstantTypeCode.Int64:
                    return string.Format("int64(0x{0:x})", (long)value);
                case ConstantTypeCode.Single:
                    return GetFloatString((float)value);
                case ConstantTypeCode.Double:
                    return GetDoubleString((double)value);
                case ConstantTypeCode.String:
                    return string.Format("\"{0}\"", value);
                case ConstantTypeCode.UInt16:
                    return string.Format("uint16(0x{0})", ((ushort)value).ToString("X4"));
                case ConstantTypeCode.UInt32:
                    return string.Format("uint32(0x{0})", ((uint)value).ToString("X8"));
                case ConstantTypeCode.UInt64:
                    return string.Format("uint64(0x{0:x})", (ulong)value);
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

            if (single == 0.0)
            {
                var bytes = BitConverter.GetBytes(single);
                if (bytes[bytes.Length - 1] == 128)
                {
                    return "float32(-0.0)";
                }
                return "float32(0.0)";
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

            if (number == 0.0)
            {
                var bytes = BitConverter.GetBytes(number);
                if (bytes[bytes.Length - 1] == 128)
                {
                    return "float64(-0.0)";
                }
                return "float64(0.0)";
            }

            return string.Format("float64({0})", number.ToString());
        }

        private object GetValue()
        {
            BlobReader reader = _readers.MdReader.GetBlobReader(_constant.Value);
            return reader.ReadConstant(TypeCode);
        }

    }
}
