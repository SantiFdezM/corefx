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
                DumpBytes(sb, Token.ToString("X4"));
            }
            sb.AppendFormat("{0,-11}", opCode);
            sb.Append(Value);
        }
    }
}