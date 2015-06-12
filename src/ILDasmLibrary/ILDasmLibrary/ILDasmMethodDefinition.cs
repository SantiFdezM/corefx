using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    public class ILDasmMethodDefinition : ILDasmObject
    {
        private readonly MethodDefinition _methodDefinition;
        private readonly MethodBodyBlock _methodBody;
        private readonly ILDasmTypeProvider _provider;
        private string _name;
        private int _rva = -1;
        private MethodSignature<string> _signature;
        private BlobReader _ilReader;
        private IEnumerable<ILInstruction> _instructions;
        private IList<Local> _locals;
        private bool isIlReaderInitialized = false;
        private bool isSignatureInitialized = false;
        private IList<ILDasmParameter> _parameters;

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

        public MethodSignature<string> Signature
        {
            get
            {
                if(!isSignatureInitialized)
                {
                    isSignatureInitialized = true;
                    _signature = ILDasmDecoder.DecodeMethodSignature(_methodDefinition, _provider);
                }
                return _signature;
            }
        }

        public bool LocalVariablesInitialized
        {
            get
            {
                return _methodBody.LocalVariablesInitialized;
            }
        }

        public bool IsEntryPoint
        {
            get
            {
                //TO DO.
                return RelativeVirtualAdress == _readers.PEReader.PEHeaders.PEHeader.AddressOfEntryPoint;
            }
        }

        public MethodAttributes Attributes
        {
            get
            {
                return _methodDefinition.Attributes;
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

        public IList<Local> Locals
        {
            get
            {
                if(_locals == null)
                {
                    _locals = ILDasmDecoder.DecodeLocalSignature(_methodBody.LocalSignature, _methodBody.LocalVariablesInitialized, _readers.MdReader, _provider);
                }
                return _locals;
            }
        }

        public IList<ILDasmParameter> Parameters
        {
            get
            {
                if(_parameters == null)
                {
                    _parameters = ILDasmDecoder.DecodeParameters(Signature, _methodDefinition.GetParameters(), _readers.MdReader);
                }
                return _parameters;
            }
        }

        public string GetDecodedSignature()
        {
            return String.Format(".method {0} {1}{2}", Signature.ReturnType, Name, _provider.GetParameterList(Signature, _methodDefinition.GetParameters()));
        }

        public string DumpMethod(bool showBytes = false)
        {
            return new ILDasmWriter().DumpMethod(this, showBytes);
        }

        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAdress);
        }
    }
}