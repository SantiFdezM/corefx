using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILDasmLibrary
{
    public class ILDasmMethodDefinition : ILDasmObject
    {
        private readonly MethodDefinition _methodDefinition;
        private readonly MethodBodyBlock _methodBody;
        private readonly ILDasmTypeProvider _provider;
        private string _name;
        private int _rva = -1;
        private string _signature;
        private string _attributes;
        private BlobReader _ilReader;
        private IEnumerable<ILInstruction> _instructions;
        private bool isIlReaderInitialized = false;

        internal ILDasmMethodDefinition(MethodDefinition methodDefinition, Readers readers) 
            : base(readers)
        {
            _methodDefinition = methodDefinition;
            _methodBody = _readers.PEReader.GetMethodBody(this.RelativeVirtualAdress);
            _provider = new ILDasmTypeProvider(readers.MdReader);
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

        internal ILDasmTypeProvider Provider
        {
            get
            {
                return _provider;
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

        public IEnumerable<ILInstruction> Instructions
        {
            get
            {
                if(_instructions == null)
                {
                    _instructions = ILDasmDecoder.DecodeMethodBody(this);
                }
                return _instructions;
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