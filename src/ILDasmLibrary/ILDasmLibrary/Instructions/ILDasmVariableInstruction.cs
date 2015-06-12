using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    internal class ILDasmVariableInstruction : ILDasmInstructionWithValue<string>
    {
        internal ILDasmVariableInstruction(OpCode opCode, string name, int token, int size) 
            : base(opCode, name, token, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat("/* {0,-4} | ", opCode.Value.ToString("X2"));
                sb.AppendFormat("{0,-16} */ ", Token.ToString("X4"));
                return;
            }
            sb.AppendFormat("{0,-10}", opCode);
            sb.Append(Value);
        }
    }
}