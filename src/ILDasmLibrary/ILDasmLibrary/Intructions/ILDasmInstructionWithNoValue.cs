using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    public class ILDasmInstructionWithNoValue :ILDasmInstruction
    {
        internal ILDasmInstructionWithNoValue(OpCode opCode, int size)
            : base(opCode, size)
        {
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat("{0,-4} | ", opCode.Value.ToString("X2"));
                return;
            }
            sb.AppendFormat("{0}", opCode);
        }
    }
}
