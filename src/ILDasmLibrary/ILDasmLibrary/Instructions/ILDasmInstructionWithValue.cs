using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public abstract class ILDasmInstructionWithValue<T> : ILInstruction
    {
        private T _value;
        private int _token;

        internal ILDasmInstructionWithValue(OpCode opCode, T value, int token, int size)
            : base(opCode, size)
        {
            _value = value;
            _token = token;
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public int Token
        {
            get
            {
                return _token;
            }
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                DumpBytes(sb, Token.ToString("X8"));
            }
            sb.AppendFormat("{0,-11}", opCode);
            sb.Append(Value.ToString());
        }

    }
}
