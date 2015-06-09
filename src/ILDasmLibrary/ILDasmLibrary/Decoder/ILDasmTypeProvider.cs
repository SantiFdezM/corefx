using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
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

        public ILDasmTypeProvider(MetadataReader reader)
        {
            this._reader = reader;
        }

        public string GetArrayType(string elementType, ArrayShape shape)
        {
            throw new NotImplementedException();
        }

        public string GetByReferenceType(string elementType)
        {
            return String.Format("{0}&", elementType);
        }

        public string GetFunctionPointerType(MethodSignature<string> signature)
        {
            throw new NotImplementedException();
        }

        public string GetGenericInstance(string genericType, ImmutableArray<string> typeArguments)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}
