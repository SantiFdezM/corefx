using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing a method definition in a type whithin an assembly.
    /// </summary>
    public class ILDasmMethodDefinition : ILDasmObject
    {
        private readonly MethodDefinition _methodDefinition;
        private readonly MethodBodyBlock _methodBody;
        private readonly ILDasmTypeProvider _provider;
        private string _name;
        private int _rva = -1;
        private MethodSignature<ILDasmType> _signature;
        private BlobReader _ilReader;
        private ImmutableArray<ILInstruction> _instructions;
        private ILDasmLocal[] _locals;
        private bool isIlReaderInitialized = false;
        private bool isSignatureInitialized = false;
        private ILDasmParameter[] _parameters;
        private int _token;
        private IEnumerable<CustomAttribute> _customAttributes;
        private IEnumerable<string> _genericParameters;
        private ILDasmTypeDefinition _typeDefinition;
        private int _methodDeclarationToken = -1;

        internal ILDasmMethodDefinition(MethodDefinition methodDefinition, int token, Readers readers, ILDasmTypeDefinition typeDefinition) 
            : base(readers)
        {
            _methodDefinition = methodDefinition;
            _token = token;
            _typeDefinition = typeDefinition;
            if(RelativeVirtualAddress != 0)
                _methodBody = _readers.PEReader.GetMethodBody(this.RelativeVirtualAddress);
            _provider = new ILDasmTypeProvider(readers.MdReader);
        }

        #region Internal Properties

        /// <summary>
        /// BlobReader that contains the msil instruction bytes.
        /// </summary>
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

        /// <summary>
        /// Type provider to solve type names and references.
        /// </summary>
        internal ILDasmTypeProvider Provider
        {
            get
            {
                return _provider;
            }
        }

        #endregion

        #region Public APIs
        /// <summary>
        /// Method name
        /// </summary>
        public string Name
        {
            get
            {
                return GetCachedValue(_methodDefinition.Name, ref _name);
            }
        }

        /// <summary>
        /// Method Relative Virtual Address, if is equal to 0 it is a virtual method and has no body.
        /// </summary>
        public int RelativeVirtualAddress
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

        /// <summary>
        /// Method Token.
        /// </summary>
        public int Token
        {
            get
            {
                return _token;
            }
        }

        /// <summary>
        /// Code Size in bytes not including headers.
        /// </summary>
        public int Size
        {
            get
            {
                return IlReader.Length;
            }
        }

        /// <summary>
        /// Method max stack capacity
        /// </summary>
        public int MaxStack
        {
            get
            {
                return _methodBody.MaxStack;
            }
        }

        /// <summary>
        /// Method Signature containing the return type, parameter count and header information.
        /// </summary>
        public MethodSignature<ILDasmType> Signature
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

        /// <summary>
        /// Boolean to know if it overrides a method definition.
        /// </summary>
        public bool IsImplementation
        {
            get
            {
                return MethodDeclarationToken != 0;
            }
        }

        /// <summary>
        /// Boolean to know if the local variables should be initialized with the "init" prefix on its signature.
        /// </summary>
        public bool LocalVariablesInitialized
        {
            get
            {
                return _methodBody.LocalVariablesInitialized;
            }
        }

        /// <summary>
        /// Boolean to know if the method has a locals declared on its body.
        /// </summary>
        public bool HasLocals
        {
            get
            {
                return !_methodBody.LocalSignature.IsNil;
            }
        }

        /// <summary>
        /// Boolean to know if the current method is the entry point of the assembly.
        /// </summary>
        public bool IsEntryPoint
        {
            get
            {
                return _token == _readers.PEReader.PEHeaders.CorHeader.EntryPointTokenOrRelativeVirtualAddress;
            }
        }

        /// <summary>
        /// Token that represents the token of the method declaration if this method overrides any. 0 if it doesn't override a declaration.
        /// </summary>
        public int MethodDeclarationToken
        {
            get
            {
                if (_methodDeclarationToken == -1)
                {
                    _methodDeclarationToken = _typeDefinition.GetMethodDeclTokenFromImplementation(Token);
                }
                return _methodDeclarationToken;
            }
        }

        /// <summary>
        /// Flags on the method (Calling convention, accesibility flags, etc.
        /// </summary>
        public MethodAttributes Attributes
        {
            get
            {
                return _methodDefinition.Attributes;
            }
        }

        /// <summary>
        /// Custom attributes declared on the method body header.
        /// </summary>
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

        /// <summary>
        /// Exception regions in the method body.
        /// </summary>
        public ImmutableArray<ExceptionRegion> ExceptionRegions
        {
            get
            {
                return _methodBody.ExceptionRegions;
            }
        }

        /// <summary>
        /// List of instructions that represent the method body.
        /// </summary>
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

        /// <summary>
        /// Parameters that the method take.
        /// </summary>
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

        /// <summary>
        /// Locals contained on the method body.
        /// </summary>
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

        /// <summary>
        /// Method generic parameters.
        /// </summary>
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

        /// <summary>
        /// Method that given an index returns a local.
        /// 
        /// Exception:
        ///     IndexOutOfBoundsException if the index is greater or equal than the number of locals or less to 0.
        /// </summary>
        /// <param name="index">Index of the local to get</param>
        /// <returns>The local in current index.</returns>
        public ILDasmLocal GetLocal(int index)
        {
            if(index < 0 || index >= Locals.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get local");
            }
            return Locals[index];
        }

        /// <summary>
        /// Method that given an index returns a parameter.
        /// 
        /// Exception:
        ///     IndexOutOfBoundsException if the index is greater or equal than the number of locals or less to 0.
        /// </summary>
        /// <param name="index">Index of the parameter to get</param>
        /// <returns>The local in current index.</returns>
        public ILDasmParameter GetParameter(int index)
        {
            if(index < 0 || index >= Parameters.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get parameter.");
            }
            return Parameters[index];
        }

        /// <summary>
        /// Method that decodes the method signature as a string with all it's flags, return type, name and parameters.
        /// </summary>
        /// <returns>Returns the method signature as a string</returns>
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

        /// <summary>
        /// Method that dumps the whole method, with it's signature, header and body as a string.
        /// </summary>
        /// <param name="showBytes">Boolean parameter that indicates if you want it to show the byte values and tokens for the instructions.</param>
        /// <returns>A string representing the whole method</returns>
        public string DumpMethod(bool showBytes = false)
        {
            return new ILDasmWriter(indentation: 0).DumpMethod(this, showBytes);
        }

        /// <summary>
        /// Method that formats the Relative Virtual Address to it's hexadecimal representation.
        /// </summary>
        /// <returns>String representing the Relative virtual Address in hexadecimal</returns>
        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAddress);
        }

        #endregion

        #region Private Methods
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
            if (i > 0)
            {
                genericParameters.Length -= 1; //Delete trailing ,
                genericParameters.Append(">");
            }
            return genericParameters.ToString();
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

        /// <summary>
        /// This Method is intended to get the accessibility flags.
        /// Since the enum doesn't have flags values, the smallest values (private, famANDAssem) will always return true.
        /// To solve this we have to check from the greatest through the smallest and the first flag it finds that way we always find the desired value.
        /// </summary>
        /// <returns>
        /// The accesibility flag as a string.
        /// </returns>
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
        #endregion
    }
}