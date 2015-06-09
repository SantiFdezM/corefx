using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;
using ILDasmLibrary.Intructions;
using ILDasmLibrary.Decoder;
using System.Reflection.Metadata.Decoding;

namespace ILDasmLibrary
{
    public struct ILDasmWriter
    {
        private string indent;

        public ILDasmWriter()
        {
            indent = "  ";
        }
        public string DumpMethod(ILDasmMethodDefinition _methodDefinition)
        {
            StringBuilder sb = new StringBuilder();
            DumpMethodDefinition(_methodDefinition, sb);
            DumpMethodBody(_methodDefinition, sb);
            sb.AppendLine("}");
            return sb.ToString();
        }
        
        private void DumpMethodDefinition(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            sb.AppendLine(String.Format(".method {0}", _methodDefinition.Name));
            sb.AppendLine("{");
        }

        private void DumpMethodBody(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            var ilReader = _methodDefinition.IlReader;
            ilReader.Reset();
            int intOperand;
            long longOperand;
            double doubleOperand;
            short shortOperand;
            float floatOperand;
            byte byteOperand;
            int ilOffset = 0;
            while (ilReader.Offset < ilReader.Length)
            {
                sb.Append(indent);
                sb.AppendFormat("IL_{0:x4}:", ilOffset);
                OpCode opCode;
                int expectedSize;
                var _byte = ilReader.ReadByte();
                /*If the byte read is 0xfe it means is a two byte instruction, 
                so since it is going to read the second byte to get the actual
                instruction it has to check that the offset is still less than the length.*/
                if (_byte == 0xfe && ilReader.Offset < ilReader.Length) 
                {
                    opCode = ILWriterHelpers.Instance.twoByteOpCodes[ilReader.ReadByte()];
                    expectedSize = 2;
                }
                else
                {
                    opCode = ILWriterHelpers.Instance.oneByteOpCodes[_byte];
                    expectedSize = 1;
                }
                sb.Append(indent);
                //sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
                int size = expectedSize;
                bool showBytes = false;
                ILDasmInstruction instruction;
                switch (opCode.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmBranchInstruction(opCode, intOperand, ilOffset, expectedSize + 4);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.InlineField:
                        size += 4;
                        sb.Append("To do InlineField");
                        intOperand = ilReader.ReadInt32();
                        var fieldInfo = GetFieldInformation(_methodDefinition, intOperand);
                        break;
                    case OperandType.InlineI:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILDasmNumericValueInstruction<int>(opCode, intOperand, -1, expectedSize + 4);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.InlineI8:
                        size += 8;
                        longOperand = ilReader.ReadInt64();
                        instruction = new ILDasmNumericValueInstruction<long>(opCode, longOperand, -1, expectedSize + 8);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.InlineMethod:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        string methodCall = SolveMethodName(_methodDefinition, intOperand);
                        instruction = new ILDasmStringInstruction<string>(opCode, methodCall, intOperand);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.InlineR:
                        size += 8;
                        doubleOperand = ilReader.ReadDouble();
                        instruction = new ILDasmNumericValueInstruction<double>(opCode, doubleOperand, -1, expectedSize + 8);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.InlineSig:
                        sb.Append("To do InlineSig");
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineString:
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        string str = GetArgumentString(_methodDefinition, intOperand);
                        instruction = new ILDasmStringInstruction<string>(opCode,string.Format(" \"{0}\"",str), intOperand);
                        instruction.Dump(sb,showBytes);
                        break;
                    case OperandType.InlineSwitch:
                        size += 4;
                        sb.Append("To do InlineSwitch");
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineTok:
                        sb.Append("To do InlineTok");
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineType:
                        sb.Append("To do InlineType");
                        size += 4;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineVar:
                        sb.Append("To do InlineVar");
                        size += 2;
                        shortOperand = ilReader.ReadInt16();
                        break;
                    case OperandType.ShortInlineBrTarget:
                        size += 1;
                        byteOperand = ilReader.ReadByte();
                        instruction = new ILDasmBranchInstruction(opCode, (int)byteOperand, ilOffset, expectedSize + 1);
                        instruction.Dump(sb,showBytes);
                        break;
                    case OperandType.ShortInlineI:
                        size += 1;
                        byteOperand = ilReader.ReadByte();
                        instruction = new ILDasmNumericValueInstruction<byte>(opCode, byteOperand, -1, expectedSize + 1);
                        instruction.Dump(sb,showBytes);
                        break;
                    case OperandType.ShortInlineR:
                        size += 4;
                        floatOperand = ilReader.ReadSingle();
                        instruction = new ILDasmNumericValueInstruction<float>(opCode, floatOperand, -1, expectedSize + 4);
                        instruction.Dump(sb, showBytes);
                        break;
                    case OperandType.ShortInlineVar:
                        sb.Append("To do ShortInlineVar");
                        size += 1;
                        byteOperand = ilReader.ReadByte();
                        break;
                    case OperandType.InlineNone:
                        instruction = new ILDasmInstructionWithNoValue(opCode, expectedSize);
                        instruction.Dump(sb,showBytes);
                        break;
                    default:
                        break;
                }
                sb.AppendLine();
                ilOffset += size;
            }
        }

        private string GetFieldInformation(ILDasmMethodDefinition _methodDefinition, int intOperand)
        {
            var handle = MetadataTokens.FieldDefinitionHandle(intOperand);
            var definition = _methodDefinition._readers.MdReader.GetFieldDefinition(handle);
            var typeHandle = definition.GetDeclaringType();
            var type = _methodDefinition._readers.MdReader.GetTypeDefinition(typeHandle);
            /* TO DO add field return type
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, new ILDasmTypeProvider(_methodDefinition._readers.MdReader));
            */
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, new ILDasmTypeProvider(_methodDefinition._readers.MdReader));
            return String.Format("{0}.{1}::{2}", GetString(_methodDefinition, type.Namespace), GetString(_methodDefinition, type.Name), GetString(_methodDefinition, definition.Name));
        }

        private string GetArgumentString(ILDasmMethodDefinition _methodDefinition, int intOperand)
        {
            var rid = intOperand >> 24;
            if (rid == 0x70)
            {
                UserStringHandle usrStr = MetadataTokens.UserStringHandle(intOperand);
                return _methodDefinition._readers.MdReader.GetUserString(usrStr);
            }
            throw new NotImplementedException("Argument String");
        }

        private string SolveMethodName(ILDasmMethodDefinition _methodDefinition, int token)
        {
            var rid = token >> 24;
            if (rid == 0x0a)
            {
                var refHandle = MetadataTokens.MemberReferenceHandle(token);
                var reference = _methodDefinition._readers.MdReader.GetMemberReference(refHandle);
                var parentToken = MetadataTokens.GetToken(reference.Parent);
                var refParent = _methodDefinition._readers.MdReader.GetTypeReference(MetadataTokens.TypeReferenceHandle(parentToken));
                var scopeToken = MetadataTokens.GetToken(refParent.ResolutionScope);
                var scope = _methodDefinition._readers.MdReader.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(scopeToken));
                return String.Format("[{0}]{1}.{2}::{3}", GetString(_methodDefinition, scope.Name), GetString(_methodDefinition, refParent.Namespace), GetString(_methodDefinition, refParent.Name), GetString(_methodDefinition, reference.Name));
            }
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = _methodDefinition._readers.MdReader.GetMethodDefinition(handle);
            var parent = _methodDefinition._readers.MdReader.GetTypeDefinition(definition.GetDeclaringType());
            return String.Format(" {0}.{1}::{2}", GetString(_methodDefinition, parent.Namespace), GetString(_methodDefinition, parent.Name), GetString(_methodDefinition, definition.Name));
        }

        private static string GetString(ILDasmMethodDefinition _methodDefinition, StringHandle handle)
        {
            return _methodDefinition._readers.MdReader.GetString(handle);
        }
    }
}
