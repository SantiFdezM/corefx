using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary.Decoder
{
    public struct ILDasmType
    {

        private StringBuilder _name;
        private bool _isValueType;
        private bool _isClassType;

        public ILDasmType(string name, bool isValueType, bool isClassType)
        {
            _name = new StringBuilder();
            _name.Append(name);
            _isValueType = isValueType;
            _isClassType = isClassType;
        }

        public string Name
        {
            get
            {
                return _name.ToString();
            }
        }

        public bool IsValueType
        {
            get
            {
                return _isValueType;
            }
        }

        public bool IsClassType
        {
            get
            {
                return _isClassType;
            }
        }

        public void Append(string str)
        {
            _name.Append(str);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool showBaseType)
        {
            string baseType = string.Empty;
            if (IsValueType)
            {
                baseType = "valuetype ";
            }
            if (IsClassType)
            {
                baseType = "class ";
            }
            if (showBaseType)
            {
                return string.Format("{0}{1}", baseType, Name);
            }
            return string.Format("{0}", Name);
        }
    }
}
