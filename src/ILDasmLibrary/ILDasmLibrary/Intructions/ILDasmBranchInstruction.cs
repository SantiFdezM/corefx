using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    class ILDasmBranchInstruction : ILDasmInstructionWithValue<int>
    {
        internal ILDasmBranchInstruction(OpCode opCode, int value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-4} | ", opCode.Value.ToString("X2"));
                sb.Append(string.Format("{0:X2}", Value));
                return;
            }
            sb.AppendFormat(opCode.OperandType == OperandType.InlineNone ? "{0}" : "{0,-10}", opCode);
            sb.Append(string.Format("IL_{0:x4}", (Token+Value+Size)));
        }
    }
}
