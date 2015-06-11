using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    public class ILDasmStringInstruction : ILDasmInstructionWithValue<string>
    {
        internal ILDasmStringInstruction(OpCode opCode,string value, int token, int size)
            : base(opCode, value, token, size)
        {
        }

        override public void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-4} | ", opCode.Value.ToString("X2"));
                sb.Append(string.Format("({0}){1}",(Token >> 24).ToString("X2"),Token.ToString("X8").Substring(2)));
                return;
            }
            sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
            if(Token >> 24 == 0x70)
            {
                sb.AppendFormat("\"{0}\"", Value);
                return;
            }
            sb.Append(Value);
        }

    }
}
