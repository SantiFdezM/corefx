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
            ICollection<ILInstruction> instructions = new Collection<ILInstruction>();
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
                int size = expectedSize;
                Console.WriteLine(opCode.Name);
                switch (opCode.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        size += 4;
                        instruction = new ILDasmBranchInstruction(opCode, ilReader.ReadInt32(), ilOffset, expectedSize + 4);
                        break;
                    case OperandType.InlineField:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        var fieldInfo = GetFieldInformation(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, fieldInfo, intOperand);
                        break;
                    case OperandType.InlineString:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        string str = GetArgumentString(mdReader, intOperand);
                        instruction = new ILDasmStringInstruction(opCode, str, intOperand);
                        break;
                    case OperandType.InlineMethod:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        string methodCall = SolveMethodName(mdReader, intOperand, provider);
                        instruction = new ILDasmStringInstruction(opCode, methodCall, intOperand);
                        break;
                    case OperandType.InlineI:
                        size += 4;
                        instruction = new ILDasmIntInstruction(opCode, ilReader.ReadInt32(), -1, expectedSize + 4);
                        break;
                    case OperandType.InlineI8:
                        size += 8;
                        instruction = new ILDasmLongInstruction(opCode, ilReader.ReadInt64(), -1, expectedSize + 8);
                        break;
                    case OperandType.InlineR:
                        size += 8;
                        instruction = new ILDasmDoubleInstruction(opCode, ilReader.ReadDouble(), -1, expectedSize + 8);
                        break;
                    case OperandType.InlineSig:
                        /*TO DO*/
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", intOperand);
                        break;
                    case OperandType.InlineSwitch:
                        /*TO DO*/
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", intOperand);
                        break;
                    case OperandType.InlineTok:
                        /*TO DO*/
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", intOperand);
                        break;
                    case OperandType.InlineType:
                        /*TO DO*/
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", intOperand);
                        break;
                    case OperandType.InlineVar:
                        /*TO DO*/
                        size += 2;
                        shortOperand = ilReader.ReadInt16();
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", shortOperand);
                        break;
                    case OperandType.ShortInlineBrTarget:
                        size += 1;
                        instruction = new ILDasmBranchInstruction(opCode, (int)ilReader.ReadByte(), ilOffset, expectedSize + 1);
                        break;
                    case OperandType.ShortInlineI:
                        size += 1;
                        instruction = new ILDasmByteInstruction(opCode, ilReader.ReadByte(), -1, expectedSize + 1);
                        break;
                    case OperandType.ShortInlineR:
                        size += 4;
                        instruction = new ILDasmFloatInstruction(opCode, ilReader.ReadSingle(), -1, expectedSize + 4);
                        break;
                    case OperandType.ShortInlineVar:
                        /*TO DO*/
                        size += 1;
                        instruction = new ILDasmStringInstruction(opCode, "TO DO", ilReader.ReadByte());
                        break;
                    case OperandType.InlineNone:
                        instruction = new ILDasmInstructionWithNoValue(opCode, expectedSize);
                        break;
                    default:
                        break;
                }
                ilOffset += size;
                instructions.Add(instruction);
            }
            return instructions.AsEnumerable<ILInstruction>();
        }

        private static string GetArgumentString(MetadataReader mdReader, int intOperand)
        {
            var rid = intOperand >> 24;
            if (rid == 0x70)
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
            string signatureValue = "";
            string parameters = "";
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
            var rid = token >> 24;
            if (rid == 0x0a)
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
            var rid = intOperand >> 24;
            if(rid == 0x0a)
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
