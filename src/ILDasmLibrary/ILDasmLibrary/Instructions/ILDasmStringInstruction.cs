using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILDasmStringInstruction : ILDasmInstructionWithValue<string>
    {
        private readonly bool _isPrintable;
        internal ILDasmStringInstruction(OpCode opCode,string value, int token, int size, bool isPrintable = true)
            : base(opCode, value, token, size)
        {
            _isPrintable = isPrintable;
        }

        override public void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                string tokenValue = string.Format("({0}){1}", (Token >> 24).ToString("X2"), Token.ToString("X8").Substring(2));
                DumpBytes(sb, tokenValue);
            }
            sb.AppendFormat("{0,-13}", opCode);
            if(Token >> 24 == 0x70)
            {
                if (_isPrintable)
                {
                    sb.AppendFormat("\"{0}\"", Value);
                    return;
                }
                sb.AppendFormat("{0}", Value);
                return;
            }
            sb.Append(Value);
        }

    }
}
