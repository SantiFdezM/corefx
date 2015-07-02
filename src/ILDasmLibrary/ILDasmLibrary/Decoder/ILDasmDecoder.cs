using ILDasmLibrary.Instructions;
using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ILDasmLibrary.Decoder
{
    public struct ILDasmDecoder
    {
        /// <summary>
        /// Method that given a token defines if it is a type reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type reference false if not</returns>
        public static bool IsTypeReference(int token)
        {
            return (token >> 24) == 0x01;
        }

        /// <summary>
        /// Method that given a token defines if it is a type definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type definition false if not</returns>
        public static bool IsTypeDefinition(int token)
        {
            return (token >> 24) == 0x02;
        }

        /// <summary>
        /// Method that given a token defines if it is a user string token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a user string false if not</returns>
        public static bool IsUserString(int token)
        {
            return (token >> 24) == 0x70;
        }

        /// <summary>
        /// Method that given a token defines if it is a member reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a member reference false if not</returns>
        public static bool IsMemberReference(int token)
        {
            return (token >> 24) == 0x0a;
        }

        /// <summary>
        /// Method that given a token defines if it is a method specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method specification false if not</returns>
        public static bool IsMethodSpecification(int token)
        {
            return (token >> 24) == 0x2b;
        }

        /// <summary>
        /// Method that given a token defines if it is a method definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method definition false if not</returns>
        public static bool IsMethodDefinition(int token)
        {
            return (token >> 24) == 0x06;
        }

        /// <summary>
        /// Method that given a token defines if it is a field definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a field definition false if not</returns>
        public static bool IsFieldDefinition(int token)
        {
            return (token >> 24) == 0x04;
        }

        /// <summary>
        /// Method that given a token defines if it is a type specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type specification false if not</returns>
        public static bool IsTypeSpecification(int token)
        {
            return (token >> 24) == 0x1b;
        }

        /// <summary>
        /// Method that given a token defines if it is a standalone signature token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a standalone signature false if not</returns>
        public static bool IsStandaloneSignature(int token)
        {
            return (token >> 24) == 0x11;
        }

        public static string DecodeCustomAttribute(CustomAttribute attribute, ILDasmMethodDefinition _methodDefinition)
        {
            switch(attribute.Constructor.Kind)
            {
                case HandleKind.MemberReference:
                case HandleKind.MethodDefinition:
                case HandleKind.MethodSpecification:
                    break;
                default:
                    throw new Exception("Decoding custom attribute handle kind: " + attribute.Constructor.Kind);
            }
            var method = SolveMethodName(_methodDefinition._readers.MdReader, MetadataTokens.GetToken(attribute.Constructor), _methodDefinition.Provider);
            return string.Format("{0} = ({1})", method, GetCustomAttributeBytes(attribute,_methodDefinition._readers.MdReader));
        }

        public static string GetCustomAttributeBytes(CustomAttribute attribute, MetadataReader MdReader)
        {
            if (attribute.Value.IsNil) return string.Empty;
            var blobReader = MdReader.GetBlobReader(attribute.Value);
            StringBuilder sb = new StringBuilder();
            while(blobReader.Offset < blobReader.Length)
            {
                sb.Append(" ");
                sb.Append(blobReader.ReadByte().ToString("X2"));
            }
            return sb.ToString();
        }

        public static MethodSignature<ILDasmType> DecodeMethodSignature(MethodDefinition _methodDefinition, ILDasmTypeProvider _provider)
        {
            return SignatureDecoder.DecodeMethodSignature(_methodDefinition.Signature, _provider);
        }

        public static IEnumerable<ILInstruction> DecodeMethodBody(ILDasmMethodDefinition _methodDefinition)
        {
            return DecodeMethodBody(_methodDefinition.IlReader, _methodDefinition._readers.MdReader, _methodDefinition.Provider, _methodDefinition);
        }

        internal static bool HasArgument(OpCode opCode)
        {
            bool isLoad = opCode == OpCodes.Ldarg || opCode == OpCodes.Ldarga || opCode == OpCodes.Ldarga_S || opCode == OpCodes.Ldarg_S;
            bool isStore = opCode == OpCodes.Starg_S || opCode == OpCodes.Starg;
            return isLoad || isStore;
        }

        private static IEnumerable<ILInstruction> DecodeMethodBody(BlobReader ilReader, MetadataReader mdReader, ILDasmTypeProvider provider, ILDasmMethodDefinition _methodDefinition)
        {
            ilReader.Reset();
            int intOperand;
            ushort shortOperand;
            int ilOffset = 0;
            ILInstruction instruction = null;
            while (ilReader.Offset < ilReader.Length)
            {
                OpCode opCode;
                int expectedSize;
                var _byte = ilReader.ReadByte();
                /*If the byte read is 0xfe it means is a two byte instruction, 
                so since it is going to read the second byte to get the actual
                instruction it has to check that the offset is still less than the length.*/
                if (_byte == 0xfe && ilReader.Offset < ilReader.Length)
                {
                    opCode = ILDecoderHelpers.Instance.twoByteOpCodes[ilReader.ReadByte()];
                    expectedSize = 2;
                }
                else
                {
                    opCode = ILDecoderHelpers.Instance.oneByteOpCodes[_byte];
                    expectedSize = 1;
                }
                switch (opCode.OperandType)
                {
                    case OperandType.InlineField:
                        intOperand = ilReader.ReadInt32();
                        string fieldInfo = GetFieldInformation(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, fieldInfo, intOperand, expectedSize + 4);
                        break;
                    case OperandType.InlineString:
                        intOperand = ilReader.ReadInt32();
                        bool isPrintable;
                        string str = GetArgumentString(mdReader, intOperand, out isPrintable);
                        instruction = new ILDasmStringInstruction(opCode, str, intOperand, expectedSize + 4, isPrintable);
                        break;
                    case OperandType.InlineMethod:
                        intOperand = ilReader.ReadInt32();
                        string methodCall = SolveMethodName(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, methodCall, intOperand, expectedSize + 4);
                        break;
                    case OperandType.InlineType:
                        intOperand = ilReader.ReadInt32();
                        string type = GetTypeInformation(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, type, intOperand, expectedSize + 4);
                        break;
                    case OperandType.InlineTok:
                        intOperand = ilReader.ReadInt32();
                        string tokenType = GetInlineTokenType(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, tokenType, intOperand, expectedSize + 4);
                        break;
                    case OperandType.InlineI:
                        instruction = new ILDasmIntInstruction(opCode, ilReader.ReadInt32(), -1, expectedSize + 4);
                        break;
                    case OperandType.InlineI8:
                        instruction = new ILDasmLongInstruction(opCode, ilReader.ReadInt64(), -1, expectedSize + 8);
                        break;
                    case OperandType.InlineR:
                        instruction = new ILDasmDoubleInstruction(opCode, ilReader.ReadDouble(), -1, expectedSize + 8);
                        break;
                    case OperandType.InlineSwitch:
                        instruction = CreateSwitchInstruction(ref ilReader, expectedSize, ilOffset, opCode);
                        break;
                    case OperandType.ShortInlineBrTarget:
                        instruction = new ILDasmShortBranchInstruction(opCode, ilReader.ReadSByte(), ilOffset, expectedSize + 1);
                        break;
                    case OperandType.InlineBrTarget:
                        instruction = new ILDasmBranchInstruction(opCode, ilReader.ReadInt32(), ilOffset, expectedSize + 4);
                        break;
                    case OperandType.ShortInlineI:
                        instruction = new ILDasmByteInstruction(opCode, ilReader.ReadByte(), -1, expectedSize + 1);
                        break;
                    case OperandType.ShortInlineR:
                        instruction = new ILDasmFloatInstruction(opCode, ilReader.ReadSingle(), -1, expectedSize + 4);
                        break;
                    case OperandType.InlineNone:
                        instruction = new ILDasmInstructionWithNoValue(opCode, expectedSize);
                        break;
                    case OperandType.ShortInlineVar:
                        byte token = ilReader.ReadByte();
                        instruction = new ILDasmShortVariableInstruction(opCode, GetVariableName(opCode, token, _methodDefinition), token, expectedSize + 1);
                        break;
                    case OperandType.InlineVar:
                        shortOperand = ilReader.ReadUInt16();
                        instruction = new ILDasmVariableInstruction(opCode, GetVariableName(opCode, shortOperand, _methodDefinition), shortOperand, expectedSize + 2);
                        break;
                    case OperandType.InlineSig:
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, GetSignature(mdReader, intOperand, provider), intOperand, expectedSize + 4);
                        break;
                    default:
                        break;
                }
                ilOffset += instruction.Size;
                yield return instruction;
            }
        }

        internal static ILDasmLocal[] DecodeLocalSignature(MethodBodyBlock methodBody, MetadataReader mdReader, ILDasmTypeProvider provider)
        {
            if (methodBody.LocalSignature.IsNil)
            {
                return new ILDasmLocal[0];
            }
            var localTypes = SignatureDecoder.DecodeLocalSignature(methodBody.LocalSignature, provider);
            ILDasmLocal[] locals = new ILDasmLocal[localTypes.Count()];
            for (int i = 0; i < localTypes.Length; i++)
            {
                string name = "V_" + i;
                locals[i] = new ILDasmLocal(name, localTypes[i].ToString());
            }
            return locals;
        }

        internal static ILDasmParameter[] DecodeParameters(MethodSignature<ILDasmType> signature, ParameterHandleCollection parameters, MetadataReader mdReader)
        {
            var types = signature.ParameterTypes;
            int requiredCount = Math.Min(signature.RequiredParameterCount, types.Length);
            if (requiredCount == 0)
            {
                return new ILDasmParameter[0];
            }
            ILDasmParameter[] result = new ILDasmParameter[requiredCount];
            for (int i = 0; i < requiredCount; i++)
            {
                var parameter = mdReader.GetParameter(parameters.ElementAt(i));
                bool isOptional = parameter.Attributes.HasFlag(ParameterAttributes.Optional);
                var parameterName = mdReader.GetString(parameter.Name);
                parameterName = ILDecoderHelpers.Instance.NormalizeString(parameterName);
                result[i] = new ILDasmParameter(parameterName, types[i].ToString(), isOptional);
            }
            return result;
        }

        internal static IEnumerable<string> DecodeGenericParameters(MethodDefinition _methodDefinition, ILDasmMethodDefinition method)
        {
            int count = method.Signature.GenericParameterCount;
            foreach (var handle in _methodDefinition.GetGenericParameters())
            {
                var parameter = method._readers.MdReader.GetGenericParameter(handle);
                yield return method._readers.MdReader.GetString(parameter.Name);
            }
        }

        internal static string DecodeType(EntityHandle catchType, ILDasmTypeProvider provider)
        {
            return SignatureDecoder.DecodeType(catchType, provider).ToString();
        }

        private static string GetSignature(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if (IsStandaloneSignature(intOperand))
            {
                var handle = MetadataTokens.StandaloneSignatureHandle(intOperand);
                var standaloneSignature = mdReader.GetStandaloneSignature(handle);
                var signature = SignatureDecoder.DecodeMethodSignature(standaloneSignature.Signature, provider);
                return string.Format("{0}{1}", GetMethodReturnType(signature), provider.GetParameterList(signature));
            }
            throw new ArgumentException("Get signature invalid token");
        }

        private static string GetVariableName(OpCode opCode,int token, ILDasmMethodDefinition _methodDefinition)
        {
            if (HasArgument(opCode))
            {
                if (_methodDefinition.Signature.Header.IsInstance)
                {
                    token--; //the first parameter is "this".
                }
                return _methodDefinition.GetParameter(token).Name;
            }
            return _methodDefinition.GetLocal(token).Name;
            
        }

        private static string GetInlineTokenType(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if(IsMethodDefinition(intOperand) || IsMethodSpecification(intOperand) || IsMemberReference(intOperand))
            {
                return "method " + SolveMethodName(mdReader, intOperand, provider);
            }
            if (IsFieldDefinition(intOperand))
            {
                return "field " + GetFieldInformation(mdReader, intOperand, provider);
            }
            return GetTypeInformation(mdReader, intOperand, provider);
        }

        private static string GetTypeInformation(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if(IsTypeReference(intOperand))
            {
                var refHandle = MetadataTokens.TypeReferenceHandle(intOperand);
                return SignatureDecoder.DecodeType(refHandle, provider).ToString();
            }
            if (IsTypeSpecification(intOperand))
            {
                var typeHandle = MetadataTokens.TypeSpecificationHandle(intOperand);
                return SignatureDecoder.DecodeType(typeHandle, provider).ToString();
            }
            var defHandle = MetadataTokens.TypeDefinitionHandle(intOperand);
            return SignatureDecoder.DecodeType(defHandle, provider).ToString();
        }

        private static ILInstruction CreateSwitchInstruction(ref BlobReader ilReader, int expectedSize, int ilOffset, OpCode opCode)
        {
            var caseNumber = ilReader.ReadUInt32();
            int[] jumps = new int[caseNumber];
            for(int i = 0; i < caseNumber; i++)
            {
                jumps[i] = ilReader.ReadInt32();
            }
            int size = 4 + expectedSize;
            size += (int)caseNumber * 4;
            return new ILDasmSwitchInstruction(opCode, ilOffset, jumps, (int)caseNumber, caseNumber, size);
        }

        private static string GetArgumentString(MetadataReader mdReader, int intOperand, out bool isPrintable)
        {
            if (IsUserString(intOperand))
            {
                UserStringHandle usrStr = MetadataTokens.UserStringHandle(intOperand);
                var str = mdReader.GetUserString(usrStr);
                str = ProcessAndNromalizeString(str, out isPrintable);
                return str;
            }
            throw new NotImplementedException("Argument String");
        }

        private static string ProcessAndNromalizeString(string str, out bool isPrintable)
        {
            isPrintable = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("bytearray (");
            foreach (var c in str)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (isPrintable && (category == UnicodeCategory.Control || category == UnicodeCategory.OtherNotAssigned || category == UnicodeCategory.OtherSymbol || c == '"'))
                {
                    var _bytes = UnicodeEncoding.Unicode.GetBytes(str);
                    foreach (var _byte in _bytes)
                    {
                        sb.Append(_byte.ToString("X2"));
                        sb.Append(" ");
                    }

                    isPrintable = false;
                    break;
                }
            }

            sb.Append(")");
            if(isPrintable)
                return str;
            return sb.ToString();
        }

        private static string GetMethodReturnType(MethodSignature<ILDasmType> signature)
        {
            StringBuilder sb = new StringBuilder();
            if (signature.Header.IsInstance)
            {
                sb.Append("instance ");
            }
            sb.Append(signature.ReturnType.ToString());
            return sb.ToString();
        }

        private static string GetMemberRef(MetadataReader mdReader, int token, ILDasmTypeProvider provider, string genericParameterSignature = "")
        {
            var refHandle = MetadataTokens.MemberReferenceHandle(token);
            var reference = mdReader.GetMemberReference(refHandle);
            var parentToken = MetadataTokens.GetToken(reference.Parent);
            string type;
            if (IsTypeSpecification(parentToken))
            {
                var typeSpecificationHandle = MetadataTokens.TypeSpecificationHandle(parentToken);
                var typeSpecification = mdReader.GetTypeSpecification(typeSpecificationHandle);
                type = SignatureDecoder.DecodeType(typeSpecificationHandle, provider).ToString();
            }
            else
            {
                var parentHandle = MetadataTokens.TypeReferenceHandle(parentToken);
                type = SignatureDecoder.DecodeType(parentHandle, provider).ToString();
            }
            string signatureValue;
            string parameters = string.Empty;
            if (reference.GetKind() == MemberReferenceKind.Method)
            {
                MethodSignature<ILDasmType> signature = SignatureDecoder.DecodeMethodSignature(reference.Signature, provider);
                signatureValue = GetMethodReturnType(signature);
                parameters = provider.GetParameterList(signature);
                return String.Format("{0} {1}::{2}{3}{4}", signatureValue, type, GetString(mdReader, reference.Name), genericParameterSignature,parameters);
            }
            signatureValue = SignatureDecoder.DecodeFieldSignature(reference.Signature, provider).ToString();
            return String.Format("{0} {1}::{2}{3}", signatureValue, type, GetString(mdReader, reference.Name), parameters);
        }

        internal static string SolveMethodName(MetadataReader mdReader, int token, ILDasmTypeProvider provider)
        {
            string genericParameters = string.Empty;
            if (IsMethodSpecification(token))
            {
                var methodHandle = MetadataTokens.MethodSpecificationHandle(token);
                var methodSpec = mdReader.GetMethodSpecification(methodHandle);
                token = MetadataTokens.GetToken(methodSpec.Method);
                genericParameters = GetGenericParametersSignature(methodSpec, provider);
            }
            if (IsMemberReference(token))
            {
                return GetMemberRef(mdReader, token, provider, genericParameters);
            }
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = mdReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            MethodSignature<ILDasmType> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var returnType = GetMethodReturnType(signature);
            var parameters = provider.GetParameterList(signature);
            var parentType = SignatureDecoder.DecodeType(parent, provider);
            return string.Format("{0} {1}::{2}{3}{4}",returnType, parentType.ToString(false), GetString(mdReader, definition.Name), genericParameters, parameters);
        }

        private static string GetGenericParametersSignature(MethodSpecification methodSpec, ILDasmTypeProvider provider)
        {
            var genericParameters = SignatureDecoder.DecodeMethodSpecificationSignature(methodSpec.Signature, provider);
            StringBuilder sb = new StringBuilder();
            int i;
            for(i = 0; i < genericParameters.Length; i++)
            {
                if(i == 0)
                {
                    sb.Append("<");
                }
                sb.Append(genericParameters[i]);
                sb.Append(",");
            }
            if(i > 0)
            {
                sb.Length--;
                sb.Append(">");
            }
            return sb.ToString();
        }

        private static string GetFieldInformation(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if(IsMemberReference(intOperand))
            {
                return GetMemberRef(mdReader, intOperand, provider);
            }
            var handle = MetadataTokens.FieldDefinitionHandle(intOperand);
            var definition = mdReader.GetFieldDefinition(handle);
            var typeHandle = definition.GetDeclaringType();
            var typeSignature = SignatureDecoder.DecodeType(typeHandle, provider);
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, provider);
            return String.Format("{0} {1}::{2}", signature.ToString(), typeSignature.ToString(false), GetString(mdReader, definition.Name));
        }

        private static string GetString(MetadataReader mdReader, StringHandle handle)
        {
            return ILDecoderHelpers.Instance.NormalizeString(mdReader.GetString(handle));
        }

        internal static string DecodeOverridenMethodName(MetadataReader mdReader, int token, ILDasmTypeProvider provider)
        {
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = mdReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            MethodSignature<ILDasmType> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var parentType = SignatureDecoder.DecodeType(parent, provider);
            return string.Format("{0}::{1}", parentType.ToString(false), GetString(mdReader, definition.Name));
        }
    }
}
