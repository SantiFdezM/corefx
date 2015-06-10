﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

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
    }
}
