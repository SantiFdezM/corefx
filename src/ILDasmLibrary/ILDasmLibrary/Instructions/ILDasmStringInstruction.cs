using System.Reflection.Emit;
using System.Text;

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
                sb.AppendFormat("/* {0,-4} | ", opCode.Value.ToString("X2"));
                string tokenValue = string.Format("({0}){1}", (Token >> 24).ToString("X2"), Token.ToString("X8").Substring(2));
                sb.AppendFormat("{0,-16} */ ", tokenValue);
            }
            sb.AppendFormat("{0,-10}", opCode);
            if(Token >> 24 == 0x70)
            {
                sb.AppendFormat("\"{0}\"", Value);
                return;
            }
            sb.Append(Value);
        }

    }
}
