using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    class ILDasmFloatInstruction : ILDasmNumericValueInstruction<float>
    {
        internal ILDasmFloatInstruction(OpCode opCode, float value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            var data = BitConverter.GetBytes(Value);
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }
    }
}
