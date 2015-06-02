using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;

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
                so since it is goint to read the second byte to get the actual
                instruction it has two check that the offset is still less than the length.*/
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
                sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
                int size = expectedSize;
                switch (opCode.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineField:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineI:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineI8:
                        size = 9;
                        longOperand = ilReader.ReadInt64();
                        break;
                    case OperandType.InlineMethod:
                        size = 5;
                        string methodCall = SolveMethodName(_methodDefinition, ilReader.ReadInt32());
                        sb.Append(methodCall);
                        break;
                    case OperandType.InlineR:
                        size = 9;
                        doubleOperand = ilReader.ReadDouble();
                        break;
                    case OperandType.InlineSig:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineString:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineSwitch:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineTok:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineType:
                        size = 5;
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineVar:
                        size = 3;
                        shortOperand = ilReader.ReadInt16();
                        break;
                    case OperandType.ShortInlineBrTarget:
                        byteOperand = ilReader.ReadByte();
                        break;
                    case OperandType.ShortInlineI:
                        byteOperand = ilReader.ReadByte();
                        break;
                    case OperandType.ShortInlineR:
                        size = 5;
                        floatOperand = ilReader.ReadSingle();
                        break;
                    case OperandType.ShortInlineVar:
                        size = 2;
                        byteOperand = ilReader.ReadByte();
                        break;
                    case OperandType.InlineNone:
                        break;
                    default:
                        break;
                }
                sb.AppendLine();
                ilOffset += size;
            }
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
