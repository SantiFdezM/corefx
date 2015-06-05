using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    public abstract class ILDasmInstructionWithValue<T> : ILDasmInstruction
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
                sb.AppendFormat("{0,-4} | ", opCode.Value.ToString("X2"));
                sb.Append(Token.ToString("X8"));
                return;
            }
            sb.AppendFormat("{0,-10}", opCode);
            sb.Append(Value.ToString());
        }

    }
}
