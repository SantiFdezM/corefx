using System;
using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    public class ILDasmMethodDefinition : ILDasmObject
    {
        private MethodDefinition _methodDefinition;
        private string _name;
        private int _rva = -1;
        private string _signature;
        private string _attributes;
        private int _maxStack = -1;
        private BlobReader _ilReader;
        private bool isIlReaderInitialized = false;
        private MethodBodyBlock _methodBody;

        internal ILDasmMethodDefinition(MethodDefinition methodDefinition, Readers readers) 
            : base(readers)
        {
            _methodDefinition = methodDefinition;
            _methodBody = _readers.PEReader.GetMethodBody(this.RelativeVirtualAdress);
        }

        internal BlobReader IlReader
        {
            get
            {
                if (!isIlReaderInitialized)
                {
                    isIlReaderInitialized = true;
                    _ilReader = _methodBody.GetILReader();
                }
                return _ilReader;
            }
        }

        public string Name
        {
            get
            {
                return GetCachedValue(_methodDefinition.Name, ref _name);
            }
        }

        public int RelativeVirtualAdress
        {
            get
            {
                if(_rva == -1)
                {
                    _rva = _methodDefinition.RelativeVirtualAddress;
                }
                return _rva;
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

        public string Attributes
        {
            get
            {
               if(_attributes == null)
                {
                    _attributes = GetAttributes();
                }
                return _attributes;
            }
        }

        public int Size
        {
            get
            {
                return _methodBody.Size;
            }
        }

        public int MaxStack
        {
            get
            {
                return _methodBody.MaxStack;
            }
        }

        public string DumpMethod()
        {
            return new ILDasmWriter().DumpMethod(this);
        }

        private string GetAttributes()
        {
            return _methodDefinition.Attributes.ToString();
        }

        private string GetSignature()
        {
            //TO DO.
            return string.Empty;
        }

        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAdress);
        }
    }
}