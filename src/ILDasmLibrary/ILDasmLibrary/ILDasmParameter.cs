namespace ILDasmLibrary
{
    /// <summary>
    /// Struct that represents a parameter object.
    /// </summary>
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

        /// <summary>
        /// Property containing the parameter name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Property containing the parameter type.
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Property that indicates whether the parameter is optional or not.
        /// </summary>
        public bool IsOptional
        {
            get
            {
                return _isOptional;
            }
        }
    }
}
