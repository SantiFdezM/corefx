using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    class ILDasmByteInstruction : ILDasmNumericValueInstruction<byte>
    {
        internal ILDasmByteInstruction(OpCode opCode, byte value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            return Value.ToString("X2");
        }
    }
}
