using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILDasmInstructionWithNoValue :ILInstruction
    {
        internal ILDasmInstructionWithNoValue(OpCode opCode, int size)
            : base(opCode, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                DumpBytes(sb, string.Empty);
            }
            sb.AppendFormat("{0}", opCode);
        }
    }
}
