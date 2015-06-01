using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary
{
    public struct ILDasmWriter
    {
        StringBuilder sb;

        public ILDasmWriter()
        {
            sb = new StringBuilder();
        }
        public string DumpMethod(ILDasmMethodDefinition _methodDefinition)
        {
            DumpMethodDefinition(_methodDefinition);
            DumpMethodBody(_methodDefinition);
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
            while (ilReader.Offset < ilReader.Length)
            {
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
                
                switch (opCode.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineField:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineI:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineI8:
                        longOperand = ilReader.ReadInt64();
                        break;
                    case OperandType.InlineMethod:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineR:
                        doubleOperand = ilReader.ReadDouble();
                        break;
                    case OperandType.InlineSig:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineString:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineSwitch:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineTok:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineType:
                        intOperand = ilReader.ReadInt32();
                        break;
                    case OperandType.InlineVar:
                        intOperand = ilReader.ReadInt16();
                        break;
                    case OperandType.ShortInlineBrTarget:
                        shortOperand = ilReader.ReadInt16();
                        break;
                    case OperandType.ShortInlineI:
                        shortOperand = ilReader.ReadInt16();
                        break;
                    case OperandType.ShortInlineR:
                        floatOperand = ilReader.ReadSingle();
                        break;
                    case OperandType.ShortInlineVar:
                        floatOperand = ilReader.ReadSingle();
                        break;
                    case OperandType.InlinePhi:
                    case OperandType.InlineNone:
                        break;
                    default:
                        break;
                }
                sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
                sb.AppendLine();
            }
        }
    }
}
