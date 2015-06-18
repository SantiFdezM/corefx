using System;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    class ILDasmLongInstruction : ILDasmNumericValueInstruction<long>
    {
        internal ILDasmLongInstruction(OpCode opCode, long value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            var data = BitConverter.GetBytes(Value);
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                DumpBytes(sb, Bytes);
            }
            sb.AppendFormat("{0,-10}", opCode);
            sb.AppendFormat("0x{0:x}",Value);
        }
    }
}
