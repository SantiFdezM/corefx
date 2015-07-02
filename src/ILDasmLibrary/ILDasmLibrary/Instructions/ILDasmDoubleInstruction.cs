using System;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    class ILDasmDoubleInstruction : ILDasmNumericValueInstruction<double>
    {
        internal ILDasmDoubleInstruction(OpCode opCode, double value, int token, int size)
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
            sb.AppendFormat("{0,-11}", opCode);
            if (double.IsNaN(Value))
            {
                var data = BitConverter.GetBytes(Value);
                sb.Append("(");
                sb.Append(BitConverter.ToString(data).Replace("-", " "));
                sb.Append(")");
                return;
            }
            sb.Append(Value.ToString());
            if (Value % 10 == 0)
            {
                sb.Append(".");
            }
        }
    }
}
