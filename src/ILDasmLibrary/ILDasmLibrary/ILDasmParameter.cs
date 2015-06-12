using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public struct ILDasmParameter
    {
        private readonly string _name;
        private readonly string _type;
        private readonly bool _isOptional;

        public ILDasmParameter(string name, string type, bool optional)
        {
            _name = name;
            _type = type;
            _isOptional = optional;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        public bool IsOptional
        {
            get
            {
                return _isOptional;
            }
        }
    }
}
