using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    abstract class ILDasmNumericValueInstruction<T> : ILDasmInstructionWithValue<T>
    {
        private string _bytes;
        internal ILDasmNumericValueInstruction(OpCode opCode, T value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat("{0,-4} | ", opCode.Value.ToString("X2"));
                sb.Append(Bytes);
                return;
            }
            base.Dump(sb);
        }

        public string Bytes
        {
            get
            {
                if(_bytes == null)
                {
                    _bytes = GetBytes();
                }
                return _bytes;
            }
        }

        protected abstract string GetBytes();
    }
}
