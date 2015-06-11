using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata.Decoding;

namespace ILDasmLibrary.Decoder
{
    internal struct ILDasmDecoder
    {
        public ILDasmDecoder()
        {
        }

        public static bool IsTypeReference(int token)
        {
            return (token >> 24) == 0x01;
        }

        public static bool IsTypeDefinition(int token)
        {
            return (token >> 24) == 0x02;
        }

        public static bool IsUserString(int token)
        {
            return (token >> 24) == 0x70;
        }

        public static bool IsMemberReference(int token)
        {
            return (token >> 24) == 0x0a;
        }

        public static bool IsMethodSpecification(int token)
        {
            return (token >> 24) == 0x2b;
        }

        public static bool IsMethodDefinition(int token)
        {
            return (token >> 24) == 0x06;
        }

        public static bool IsFieldDefinition(int token)
        {
            return (token >> 24) == 0x04;
        }

        public static MethodSignature<string> DecodeMethodSignature(MethodDefinition _methodDefinition, ILDasmTypeProvider _provider)
        {
            return SignatureDecoder.DecodeMethodSignature(_methodDefinition.Signature, _provider);
        }

        public static IEnumerable<ILInstruction> DecodeMethodBody(ILDasmMethodDefinition _methodDefinition)
        {
            return DecodeMethodBody(_methodDefinition.IlReader, _methodDefinition._readers.MdReader, _methodDefinition.Provider);
        }

        private static IEnumerable<ILInstruction> DecodeMethodBody(BlobReader ilReader, MetadataReader mdReader, ILDasmTypeProvider provider)
        {
            ilReader.Reset();
            int intOperand;
            short shortOperand;
            int ilOffset = 0;
            IList<ILInstruction> instructions = new List<ILInstruction>();
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
                        string str = GetArgumentString(mdReader, intOperand);
                        instruction = new ILDasmStringInstruction(opCode, str, intOperand, expectedSize + 4);
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
                        /*TO DO SEPARATE BRANCH TO SHORT BRANCH TARGET*/
                    case OperandType.ShortInlineBrTarget:
                        instruction = new ILDasmBranchInstruction(opCode, (int)ilReader.ReadByte(), ilOffset, expectedSize + 1);
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
                        /*TO DO*/
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", ilReader.ReadByte(), expectedSize + 1);
                        break;
                    case OperandType.InlineVar:
                        /*TO DO*/
                        shortOperand = ilReader.ReadInt16();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", shortOperand, expectedSize + 2);
                        break;
                    case OperandType.InlineSig:
                        /*TO DO*/
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", intOperand, expectedSize + 4);
                        break;
                    default:
                        break;
                }
                ilOffset += instruction.Size;
                instructions.Add(instruction);
            }
            return instructions.AsEnumerable<ILInstruction>();
        }

        private static string GetInlineTokenType(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if(IsMethodDefinition(intOperand) || IsMethodSpecification(intOperand) || IsMemberReference(intOperand))
            {
                return SolveMethodName(mdReader, intOperand, provider);
            }
            if (IsFieldDefinition(intOperand))
            {
                return GetFieldInformation(mdReader, intOperand, provider);
            }
            return GetTypeInformation(mdReader, intOperand, provider);
        }

        private static string GetTypeInformation(MetadataReader mdReader, int intOperand, ILDasmTypeProvider provider)
        {
            if(IsTypeReference(intOperand))
            {
                var refHandle = MetadataTokens.TypeReferenceHandle(intOperand);
                return SignatureDecoder.DecodeType(refHandle, provider);
            }
            var defHandle = MetadataTokens.TypeDefinitionHandle(intOperand);
            return SignatureDecoder.DecodeType(defHandle, provider);
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

        private static string GetArgumentString(MetadataReader mdReader, int intOperand)
        {
            if (IsUserString(intOperand))
            {
                UserStringHandle usrStr = MetadataTokens.UserStringHandle(intOperand);
                return mdReader.GetUserString(usrStr);
            }
            throw new NotImplementedException("Argument String");
        }

        private static string GetMethodReturnType(MethodSignature<string> signature, ILDasmTypeProvider provider)
        {
            StringBuilder sb = new StringBuilder();
            if (signature.Header.IsInstance)
            {
                sb.Append("instance ");
            }
            sb.Append(signature.ReturnType);
            return sb.ToString();
        }

        private static string GetMemberRef(MetadataReader mdReader, int token, ILDasmTypeProvider provider)
        {
            var refHandle = MetadataTokens.MemberReferenceHandle(token);
            var reference = mdReader.GetMemberReference(refHandle);
            var parentToken = MetadataTokens.GetToken(reference.Parent);
            var refParent = mdReader.GetTypeReference(MetadataTokens.TypeReferenceHandle(parentToken));
            var scopeToken = MetadataTokens.GetToken(refParent.ResolutionScope);
            var scope = mdReader.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(scopeToken));
            string signatureValue;
            string parameters = string.Empty;
            if (reference.GetKind() == MemberReferenceKind.Method)
            {
                MethodSignature<string> signature = SignatureDecoder.DecodeMethodSignature(reference.Signature, provider);
                signatureValue = GetMethodReturnType(signature, provider);
                parameters = provider.GetParameterList(signature);
            }
            else
            {
                signatureValue = SignatureDecoder.DecodeFieldSignature(reference.Signature, provider);
            }
            return String.Format("{0} [{1}]{2}.{3}::{4}{5}", signatureValue, GetString(mdReader, scope.Name), GetString(mdReader, refParent.Namespace), GetString(mdReader, refParent.Name), GetString(mdReader, reference.Name), parameters);
        }

        private static string SolveMethodName(MetadataReader mdReader, int token, ILDasmTypeProvider provider)
        {
            if (IsMethodSpecification(token))
            {
                var methodHandle = MetadataTokens.MethodSpecificationHandle(token);
                var methodSpec = mdReader.GetMethodSpecification(methodHandle);
                token = MetadataTokens.GetToken(methodSpec.Method);
            }
            if (IsMemberReference(token))
            {
                return GetMemberRef(mdReader, token, provider);
            }
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = mdReader.GetMethodDefinition(handle);
            var parent = mdReader.GetTypeDefinition(definition.GetDeclaringType());
            MethodSignature<string> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var returnType = GetMethodReturnType(signature, provider);
            var parameters = provider.GetParameterList(signature);
            return String.Format("{0} {1}.{2}::{3}{4}", returnType, GetString(mdReader, parent.Namespace), GetString(mdReader, parent.Name), GetString(mdReader, definition.Name), parameters);
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
            var type = mdReader.GetTypeDefinition(typeHandle);
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, provider);
            return String.Format("{0} {1}.{2}::{3}",signature, GetString(mdReader, type.Namespace), GetString(mdReader, type.Name), GetString(mdReader, definition.Name));
        }

        private static string GetString(MetadataReader mdReader, StringHandle handle)
        {
            return mdReader.GetString(handle);
        }
    }
}
