using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
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
        private MethodSignature<string> _signature;
        private BlobReader _ilReader;
        private ImmutableArray<ILInstruction> _instructions;
        private ILDasmLocal[] _locals;
        private bool isIlReaderInitialized = false;
        private bool isSignatureInitialized = false;
        private ILDasmParameter[] _parameters;
        private int _token;
        private IEnumerable<CustomAttribute> _customAttributes;

        internal ILDasmMethodDefinition(MethodDefinition methodDefinition, int token, Readers readers) 
            : base(readers)
        {
            _methodDefinition = methodDefinition;
            _token = token;
            if(RelativeVirtualAdress != 0)
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

        public int Token
        {
            get
            {
                return _token;
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

        public bool HasLocals
        {
            get
            {
                return !_methodBody.LocalSignature.IsNil;
            }
        }

        public bool IsEntryPoint
        {
            get
            {
                return _token == _readers.PEReader.PEHeaders.CorHeader.EntryPointTokenOrRelativeVirtualAddress;
            }
        }

        public MethodAttributes Attributes
        {
            get
            {
                return _methodDefinition.Attributes;
            }
        }

        public IEnumerable<CustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttributes == null)
                {
                    _customAttributes = PopulateCustomAttributes();
                }
                return _customAttributes;
            }
        }

        public ImmutableArray<ExceptionRegion> ExceptionRegions
        {
            get
            {
                return _methodBody.ExceptionRegions;
            }
        }

        public int Size
        {
            get
            {
                return IlReader.Length;
            }
        }

        public int MaxStack
        {
            get
            {
                return _methodBody.MaxStack;
            }
        }

        public ImmutableArray<ILInstruction> Instructions
        {
            get
            {
                if(_instructions == null)
                {
                    _instructions = ILDasmDecoder.DecodeMethodBody(this).ToImmutableArray<ILInstruction>();
                }
                return _instructions;
            }
        }

        public IEnumerable<ILDasmParameter> Parameters
        {
            get
            {
                return GetParameters();
            }
        }

        

        public IEnumerable<ILDasmLocal> Locals
        {
            get
            {
                return GetLocals();
            }
        }

        private ILDasmParameter[] GetParameters()
        {
            if (_parameters == null)
            {
                _parameters = ILDasmDecoder.DecodeParameters(Signature, _methodDefinition.GetParameters(), _readers.MdReader);
            }
            return _parameters;
        }

        private ILDasmLocal[] GetLocals()
        {
            if (_locals == null)
            {
                _locals = ILDasmDecoder.DecodeLocalSignature(_methodBody, _readers.MdReader, _provider);
            }
            return _locals;
        }

        public ILDasmLocal GetLocal(int index)
        {
            if(index < 0 || index >= GetLocals().Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get local");
            }
            return GetLocals()[index];
        }

        public ILDasmParameter GetParameter(int index)
        {
            if(index < 0 || index >= GetParameters().Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get parameter.");
            }
            return GetParameters()[index];
        }

        public string GetDecodedSignature()
        {
            return String.Format(".method /*{0}*/ {1} {2} {3}{4}",Token.ToString("X8"),Attributes.ToString(), Signature.ReturnType, Name, _provider.GetParameterList(Signature, _methodDefinition.GetParameters()));
        }

        public string DumpMethod(bool showBytes = false)
        {
            return new ILDasmWriter().DumpMethod(this, showBytes);
        }

        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAdress);
        }

        private IEnumerable<CustomAttribute> PopulateCustomAttributes()
        {
            foreach(var handle in _methodDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return attribute;
            }
        }
    }
}