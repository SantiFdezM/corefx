using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    class ILDasmShortBranchInstruction : ILDasmInstructionWithValue<sbyte>
    {
        internal ILDasmShortBranchInstruction(OpCode opCode, sbyte value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                DumpBytes(sb, Value.ToString("X2"));
            }
            sb.AppendFormat("{0,-11}", opCode);
            sb.Append(string.Format("IL_{0:x4}", (Token + Value + Size)));
        }
    }
}
