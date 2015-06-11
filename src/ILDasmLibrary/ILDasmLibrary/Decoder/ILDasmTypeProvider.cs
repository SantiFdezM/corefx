using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary.Decoder
{
    public class ILDasmTypeProvider : ISignatureTypeProvider<string>
    {
        private readonly MetadataReader _reader;

        public MetadataReader Reader
        {
            get
            {
                return _reader;
            }
        }

        public ILDasmTypeProvider(MetadataReader reader)
        {
            this._reader = reader;
        }

        private string GetName(TypeReference reference)
        {
            if (reference.Namespace.IsNil)
            {
                return Reader.GetString(reference.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(reference.Namespace), Reader.GetString(reference.Name));
        }

        private string GetName(TypeDefinition type)
        {
            if (type.Namespace.IsNil)
            {
                return Reader.GetString(type.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(type.Namespace), Reader.GetString(type.Name));
        }

        private string GetFullName(TypeDefinitionHandle handle)
        {
            return GetFullName(Reader.GetTypeDefinition(handle));
        }

        private string GetFullName(TypeDefinition type)
        {
            var declaringType = type.GetDeclaringType();

            if (declaringType.IsNil)
            {
                return GetName(type);
            }
            return String.Format("{0}/{1}", GetFullName(declaringType), GetName(type));
        }

        private string GetFullName(TypeReferenceHandle handle)
        {
            return GetFullName(Reader.GetTypeReference(handle));
        }

        private string GetFullName(TypeReference reference)
        {
            Handle resolutionScope = reference.ResolutionScope;
            string name = GetName(reference);
            switch (resolutionScope.Kind)
            {
                case HandleKind.ModuleReference:
                    return String.Format("[.module {0}]{1}", Reader.GetString(Reader.GetModuleReference((ModuleReferenceHandle)resolutionScope).Name), name);
                case HandleKind.AssemblyReference:
                    return String.Format("[{0}]{1}", Reader.GetString(Reader.GetAssemblyReference((AssemblyReferenceHandle)resolutionScope).Name), name);
                case HandleKind.TypeReference:
                    return String.Format("{0}/{1}", GetFullName((TypeReferenceHandle)resolutionScope), name);
                default:
                    return name;
            }
        }

        public string GetArrayType(string elementType, ArrayShape shape)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(elementType);
            sb.Append("[");
            for(int i = 0; i < shape.Rank; i++)
            {
                int lowerBound = 0;
                if(i < shape.LowerBounds.Length)
                {
                    lowerBound = shape.LowerBounds[i];
                    sb.Append(lowerBound);
                    sb.Append("...");
                }

                if(i < shape.Sizes.Length)
                {
                    sb.Append(lowerBound + shape.Sizes[i] - 1);
                }

                if( i < shape.Rank -1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public string GetByReferenceType(string elementType)
        {
            return String.Format("{0}&", elementType);
        }

        public string GetFunctionPointerType(MethodSignature<string> signature)
        {
            return String.Format("method {0}*{1}", signature.ReturnType, GetParameterList(signature));
        }

        public string GetGenericInstance(string genericType, ImmutableArray<string> typeArguments)
        {
            return String.Format("{0}<{1}>", genericType, String.Join(",", typeArguments));
        }

        public string GetGenericMethodParameter(int index)
        {
            return String.Format("!!{0}", index);
        }

        public string GetGenericTypeParameter(int index)
        {
            return String.Format("!{0}", index);
        }

        public string GetModifiedType(string unmodifiedType, ImmutableArray<CustomModifier<string>> customModifiers)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(unmodifiedType);
            sb.Append(" ");

            foreach(var modifier in customModifiers)
            {
                sb.Append(modifier.IsRequired ? "modreq(" : "modopt(");
                sb.Append(modifier.Type);
                sb.Append(")");
            }

            return sb.ToString();
        }

        public string GetPinnedType(string elementType)
        {
            return String.Format("{0} pinned", elementType);
        }

        public string GetPointerType(string elementType)
        {
            return String.Format("{0}*", elementType);
        }

        public string GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean:
                    return "bool";
                case PrimitiveTypeCode.Byte:
                    return "uint8";
                case PrimitiveTypeCode.SByte:
                    return "int8";
                case PrimitiveTypeCode.Char:
                    return "char";
                case PrimitiveTypeCode.Single:
                    return "float";
                case PrimitiveTypeCode.Double:
                    return "double";
                case PrimitiveTypeCode.Int16:
                    return "int16";
                case PrimitiveTypeCode.Int32:
                    return "int32";
                case PrimitiveTypeCode.Int64:
                    return "int64";
                case PrimitiveTypeCode.UInt16:
                    return "uint16";
                case PrimitiveTypeCode.UInt32:
                    return "uint32";
                case PrimitiveTypeCode.UInt64:
                    return "uint64";
                case PrimitiveTypeCode.IntPtr:
                    return "native int";
                case PrimitiveTypeCode.UIntPtr:
                    return "native uint";
                case PrimitiveTypeCode.Object:
                    return "object";
                case PrimitiveTypeCode.String:
                    return "string";
                case PrimitiveTypeCode.TypedReference:
                    return "typedref";
                case PrimitiveTypeCode.Void:
                    return "void";
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException("invalid typeCode");
            }
        }

        public string GetSZArrayType(string elementType)
        {
            return String.Format("{0}[]", elementType);
        }

        public string GetTypeFromDefinition(TypeDefinitionHandle handle)
        {
            return GetFullName(Reader.GetTypeDefinition(handle));
        }

        public string GetTypeFromReference(TypeReferenceHandle handle)
        {
            return GetFullName(Reader.GetTypeReference(handle));
        }

        public string GetParameterList(MethodSignature<string> signature, ParameterHandleCollection? parameters = null)
        {
            ImmutableArray<string> types = signature.ParameterTypes;
            if (types.IsEmpty)
            {
                return "()";
            }
            int requiredCount = Math.Min(signature.RequiredParameterCount, types.Length);
            string[] parameterNames = GetParameterNames(parameters, requiredCount);
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            int i = 0;
            for (; i < requiredCount; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(types[i]);
                if (parameterNames != null)
                {
                    sb.AppendFormat(" {0}", parameterNames[i]);
                }
            }

            if (i < types.Length)
            {
                sb.Append("...,");
            }
            for (; i < types.Length; i++)
            {
                sb.Append(types[i]);
                if(i < types.Length -1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        private string[] GetParameterNames(ParameterHandleCollection? parameters, int requiredCount)
        {
            if(parameters == null || requiredCount == 0)
            {
                return null;
            }
            string[] parameterNames = new string[requiredCount];
            foreach(var handle in parameters)
            {
                Parameter parameter = Reader.GetParameter(handle);
                if(parameter.SequenceNumber > 0 && parameter.SequenceNumber <= requiredCount)
                {
                    parameterNames[parameter.SequenceNumber - 1] = Reader.GetString(parameter.Name);
                }
            }
            return parameterNames;
        }
    }
}
