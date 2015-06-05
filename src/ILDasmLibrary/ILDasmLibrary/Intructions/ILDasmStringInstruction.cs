using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    public class ILDasmStringInstruction<T> : ILDasmInstructionWithValue<T>
    {
        internal ILDasmStringInstruction(OpCode opCode,T value, int token)
            : base(opCode, value, token, 5)
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
            sb.Append(Value.ToString());
        }

    }
}
