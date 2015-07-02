﻿using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Text;

namespace ILDasmLibrary.Decoder
{
    public struct ILDasmTypeProvider : ISignatureTypeProvider<ILDasmType>
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
            _reader = reader;
        }

        #region Public APIs

        public ILDasmType GetArrayType(ILDasmType elementType, ArrayShape shape)
        {
            elementType.Append("[");
            for(int i = 0; i < shape.Rank; i++)
            {
                int lowerBound = 0;
                if(i < shape.LowerBounds.Length)
                {
                    lowerBound = shape.LowerBounds[i];
                    elementType.Append(lowerBound.ToString());
                    elementType.Append("...");
                }

                if(i < shape.Sizes.Length)
                {
                    elementType.Append((lowerBound + shape.Sizes[i] - 1).ToString());
                }

                if( i < shape.Rank -1)
                {
                    elementType.Append(",");
                }
            }
            elementType.Append("]");
            return elementType;
        }

        public ILDasmType GetByReferenceType(ILDasmType elementType)
        {
            elementType.Append("&");
            return elementType;
        }

        public ILDasmType GetFunctionPointerType(MethodSignature<ILDasmType> signature)
        {
            ILDasmType type = new ILDasmType("method ", false, false);
            type.Append(signature.ReturnType.ToString());
            type.Append("*");
            type.Append(GetParameterList(signature));
            return type;
        }

        public ILDasmType GetGenericInstance(ILDasmType genericType, ImmutableArray<ILDasmType> typeArguments)
        {
            genericType.Append("<");
            for(int i = 0; i < typeArguments.Length; i++)
            {
                var typeArgument = typeArguments[i];
                genericType.Append(typeArgument.ToString());
                if(i < typeArguments.Length - 1)
                {
                    genericType.Append(",");
                }
            }
            genericType.Append(">");
            return genericType;
        }

        public ILDasmType GetGenericMethodParameter(int index)
        {
            var type = new ILDasmType("", false, false);
            type.Append("!!");
            type.Append(index.ToString());
            return type;
        }

        public ILDasmType GetGenericTypeParameter(int index)
        {
            var type = new ILDasmType("", false, false);
            type.Append("!");
            type.Append(index.ToString());
            return type;
        }

        public ILDasmType GetModifiedType(ILDasmType unmodifiedType, ImmutableArray<CustomModifier<ILDasmType>> customModifiers)
        {
            unmodifiedType.Append(" ");

            foreach(var modifier in customModifiers)
            {
                unmodifiedType.Append(modifier.IsRequired ? "modreq(" : "modopt(");
                unmodifiedType.Append(modifier.Type.ToString());
                unmodifiedType.Append(")");
            }

            return unmodifiedType;
        }

        public ILDasmType GetPinnedType(ILDasmType elementType)
        {
            elementType.Append(" pinned");
            return elementType;
        }

        public ILDasmType GetPointerType(ILDasmType elementType)
        {
            elementType.Append("*");
            return elementType;
        }

        public ILDasmType GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string str;
            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean:
                    str = "bool";
                    break;
                case PrimitiveTypeCode.Byte:
                    str = "uint8";
                    break;
                case PrimitiveTypeCode.SByte:
                    str = "int8";
                    break;
                case PrimitiveTypeCode.Char:
                    str = "char";
                    break;
                case PrimitiveTypeCode.Single:
                    str = "float32";
                    break;
                case PrimitiveTypeCode.Double:
                    str = "float64";
                    break;
                case PrimitiveTypeCode.Int16:
                    str = "int16";
                    break;
                case PrimitiveTypeCode.Int32:
                    str = "int32";
                    break;
                case PrimitiveTypeCode.Int64:
                    str = "int64";
                    break;
                case PrimitiveTypeCode.UInt16:
                    str = "uint16";
                    break;
                case PrimitiveTypeCode.UInt32:
                    str = "uint32";
                    break;
                case PrimitiveTypeCode.UInt64:
                    str = "uint64";
                    break;
                case PrimitiveTypeCode.IntPtr:
                    str = "native int";
                    break;
                case PrimitiveTypeCode.UIntPtr:
                    str = "native uint";
                    break;
                case PrimitiveTypeCode.Object:
                    str = "object";
                    break;
                case PrimitiveTypeCode.String:
                    str = "string";
                    break;
                case PrimitiveTypeCode.TypedReference:
                    str = "typedref";
                    break;
                case PrimitiveTypeCode.Void:
                    str = "void";
                    break;
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException("invalid typeCode");
            }
            return new ILDasmType(str, false, false);
        }

        public ILDasmType GetSZArrayType(ILDasmType elementType)
        {
            elementType.Append("[]");
            return elementType;
        }

        public ILDasmType GetTypeFromDefinition(TypeDefinitionHandle handle)
        {
            bool isClass = false;
            bool isValueType = false;
            return new ILDasmType(GetFullName(Reader.GetTypeDefinition(handle), ref isClass, ref isValueType),isValueType, isClass);
        }

        public ILDasmType GetTypeFromReference(TypeReferenceHandle handle)
        {
            return new ILDasmType(GetFullName(Reader.GetTypeReference(handle)),false, false);
        }

        public string GetParameterList(MethodSignature<ILDasmType> signature, ParameterHandleCollection? parameters = null)
        {
            ImmutableArray<ILDasmType> types = signature.ParameterTypes;
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
                sb.Append(types[i].ToString());
                if (parameterNames != null)
                {
                    sb.AppendFormat(" {0}", ILDecoderHelpers.Instance.NormalizeString(parameterNames[i]));
                }
            }

            if (i < types.Length)
            {
                sb.Append("...,");
            }
            for (; i < types.Length; i++)
            {
                sb.Append(types[i].ToString());
                if(i < types.Length -1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region Private helper methods

        internal string[] GetParameterNames(ParameterHandleCollection? parameters, int requiredCount)
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

        private string GetName(TypeReference reference)
        {
            if (reference.Namespace.IsNil)
            {
                return Reader.GetString(reference.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(reference.Namespace), Reader.GetString(reference.Name));
        }

        private string GetName(TypeDefinition type, ref bool isClass, ref bool isValueType)
        {
            var declType = type.BaseType;
            string declName = string.Empty;
            if (!declType.IsNil)
            {
                if (declType.Kind == HandleKind.TypeDefinition)
                {
                    declName = GetName(_reader.GetTypeDefinition((TypeDefinitionHandle)declType), ref isClass, ref isValueType);
                }
                if (declType.Kind == HandleKind.TypeReference)
                {
                    Debug.Assert(true);
                    declName = GetFullName((TypeReferenceHandle)declType);
                }
                if (declName == "System.ValueType") //ValueType inherits from System.Object as well so we have to reset isClass to false
                {
                    isValueType = true;
                    isClass = false;
                }
                if (declName == "System.Object" && !isValueType)
                {
                    isClass = true;
                }
            }
            else //interfaces indirectly inherit from System.Object
            {
                isClass = true;
            }
            if (type.Namespace.IsNil)
            {
                return Reader.GetString(type.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(type.Namespace), Reader.GetString(type.Name));
        }

        private string GetFullName(TypeDefinitionHandle handle, ref bool isClass, ref bool isValueType)
        {
            return GetFullName(Reader.GetTypeDefinition(handle), ref isClass, ref isValueType);
        }

        private string GetFullName(TypeDefinition type, ref bool isClass, ref bool isValueType)
        {
            var declaringType = type.GetDeclaringType();

            if (declaringType.IsNil)
            {
                return GetName(type, ref isClass, ref isValueType);
            }
            return String.Format("{0}/{1}", GetFullName(declaringType, ref isClass, ref isValueType), GetName(type, ref isClass, ref isValueType));
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
                case HandleKind.TypeSpecification:
                    throw new ArgumentException("Check this type");
                default:
                    return name;
            }
        }

        #endregion
    }
}
