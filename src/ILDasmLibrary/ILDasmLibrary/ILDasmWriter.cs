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
        private StringBuilder sb;
        private string indent;

        public ILDasmWriter()
        {
            sb = new StringBuilder();
            indent = "  ";
        }
        public string DumpMethod(ILDasmMethodDefinition _methodDefinition)
        {
            DumpMethodDefinition(_methodDefinition);
            DumpMethodBody(_methodDefinition);
            sb.AppendLine("}");
            return sb.ToString();
        }
        
        private void DumpMethodDefinition(ILDasmMethodDefinition _methodDefinition)
        {
            sb.AppendFormat(".method {0}", _methodDefinition.Name);
            sb.AppendLine("\n{");
        }

        private void DumpMethodBody(ILDasmMethodDefinition _methodDefinition)
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
                if(_byte == 0xfe && ilReader.Offset < ilReader.Length)
                {
                    opCode = ILWriterHelpers.Instance._twoByteOpCodes[ilReader.ReadByte()];
                    expectedSize = 2;
                }
                else
                {
                    opCode = ILWriterHelpers.Instance._oneByteOpCodes[_byte];
                    expectedSize = 1;
                }
                sb.Append(indent);
                sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
                int size = 2;
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
                        byteOperand = ilReader.ReadByte();
                        break;
                    case OperandType.InlinePhi:
                    case OperandType.InlineNone:
                        size = 1;
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

        private string GetString(ILDasmMethodDefinition _methodDefinition, StringHandle handle)
        {
            return _methodDefinition._readers.MdReader.GetString(handle);
        }
    }
}
