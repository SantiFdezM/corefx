using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    class ILDasmNumericValueInstruction<T> : ILDasmInstructionWithValue<T>
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

        private string GetBytes()
        {
            var type = Value.GetType();
            if(type == typeof(int))
            {
                int value = int.Parse(Value.ToString());
                return value.ToString("X2");
            }
            if(type == typeof(short))
            {
                short value = short.Parse(Value.ToString());
                return value.ToString("X2");
            }
            if(type == typeof(byte))
            {
                byte value = byte.Parse(Value.ToString());
                return value.ToString("X2");
            }
            byte[] data;
            if(type == typeof(long))
            {
                long value = long.Parse(Value.ToString());
                data = BitConverter.GetBytes(value);
                return BitConverter.ToString(data).Replace("-", string.Empty);
            }
            if (type == typeof(double))
            {
                double value = double.Parse(Value.ToString());
                data = BitConverter.GetBytes(value);
                return BitConverter.ToString(data).Replace("-", string.Empty);
            }
            if (type == typeof(float))
            {
                float value = float.Parse(Value.ToString());
                data = BitConverter.GetBytes(value);
                return BitConverter.ToString(data).Replace("-", string.Empty);
            }
            throw new NotImplementedException("Not implemented type: " + type.FullName);
        }
    }
}
