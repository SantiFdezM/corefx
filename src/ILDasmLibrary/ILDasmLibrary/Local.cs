namespace ILDasmLibrary
{
    public struct ILDasmLocal
    {
        private readonly string _name;
        private readonly string _type;

        public ILDasmLocal(string name, string type)
        {
            _name = name;
            _type = type;
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
    }
}