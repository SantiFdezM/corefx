using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

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
        private IEnumerable<string> _genericParameters;

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

        public ILDasmParameter[] Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = ILDasmDecoder.DecodeParameters(Signature, _methodDefinition.GetParameters(), _readers.MdReader);
                }
                return _parameters;
            }
        }

        

        public ILDasmLocal[] Locals
        {
            get
            {
                if (_locals == null)
                {
                    _locals = ILDasmDecoder.DecodeLocalSignature(_methodBody, _readers.MdReader, _provider);
                }
                return _locals;
            }
        }

        public IEnumerable<string> GenericParameters
        {
            get
            {
                if(_genericParameters == null)
                {
                    _genericParameters = ILDasmDecoder.DecodeGenericParameters(_methodDefinition, this);
                }
                return _genericParameters;
            }
        }

        public ILDasmLocal GetLocal(int index)
        {
            if(index < 0 || index >= Locals.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get local");
            }
            return Locals[index];
        }

        public ILDasmParameter GetParameter(int index)
        {
            if(index < 0 || index >= Parameters.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get parameter.");
            }
            return Parameters[index];
        }

        public string GetDecodedSignature()
        {
            string attributes = GetAttributesForSignature();
            StringBuilder signature = new StringBuilder();
            if (Signature.Header.IsInstance)
            {
                signature.Append("instance ");
            }
            signature.Append(Signature.ReturnType);
            return String.Format(".method /*{0}*/{1}{2} {3}{4}{5}", Token.ToString("X8"), attributes, signature.ToString(), Name, GetGenericParametersString(),_provider.GetParameterList(Signature, _methodDefinition.GetParameters()));
        }

        private string GetGenericParametersString()
        {
            int i = 0;
            StringBuilder genericParameters = new StringBuilder();
            foreach (var genericParameter in GenericParameters)
            {
                if (i == 0)
                {
                    genericParameters.Append("<");
                }
                genericParameters.Append(genericParameter);
                genericParameters.Append(",");
                i++;
            }
            if(i > 0)
            {
                genericParameters.Length -= 1; //Delete trailing ,
                genericParameters.Append(">");
            }
            return genericParameters.ToString();
        }

        public string DumpMethod(bool showBytes = false)
        {
            return new ILDasmWriter(indentation: 0).DumpMethod(this, showBytes);
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

        private string GetAttributesForSignature()
        {
            return string.Format("{0}{1}", GetAccessibilityFlags(), GetContractFlags());
        }

        private string GetContractFlags()
        {
            StringBuilder sb = new StringBuilder();
            if (Attributes.HasFlag(MethodAttributes.HideBySig))
            {
                sb.Append("hidebysig ");
            }
            if (Attributes.HasFlag(MethodAttributes.Static))
            {
                sb.Append("static ");
            }
            if(Attributes.HasFlag(MethodAttributes.NewSlot))
            {
                sb.Append("newslot ");
            }
            if (Attributes.HasFlag(MethodAttributes.SpecialName))
            {
                sb.Append("specialname ");
            }
            if (Attributes.HasFlag(MethodAttributes.RTSpecialName))
            {
                sb.Append("rtspecialname ");
            }
            if (Attributes.HasFlag(MethodAttributes.Abstract))
            {
                sb.Append("abstract ");
            }
            if (Attributes.HasFlag(MethodAttributes.CheckAccessOnOverride))
            {
                sb.Append("strict ");
            }
            if (Attributes.HasFlag(MethodAttributes.Virtual))
            {
                sb.Append("virtual ");
            }
            if (Attributes.HasFlag(MethodAttributes.Final))
            {
                sb.Append("final ");
            }
            if (Attributes.HasFlag(MethodAttributes.PinvokeImpl))
            {
                sb.Append("pinvokeimpl");
            }
            return sb.ToString();
        }

        private string GetAccessibilityFlags()
        {
            if (Attributes.HasFlag(MethodAttributes.Public))
            {
                return "public ";
            }
            if (Attributes.HasFlag(MethodAttributes.FamORAssem))
            {
                return "famorassem ";
            }
            if (Attributes.HasFlag(MethodAttributes.Family))
            {
                return "family ";
            }
            if (Attributes.HasFlag(MethodAttributes.Assembly))
            {
                return "assembly ";
            }
            if (Attributes.HasFlag(MethodAttributes.FamANDAssem))
            {
                return "famandassem ";
            }
            if (Attributes.HasFlag(MethodAttributes.Private))
            {
                return "private ";
            }
            return string.Empty;
        }
    }
}