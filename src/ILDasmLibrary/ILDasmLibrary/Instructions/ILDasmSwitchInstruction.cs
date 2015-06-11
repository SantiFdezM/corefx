using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    class ILDasmSwitchInstruction : ILDasmInstructionWithValue<uint>
    {
        private readonly int[] _jumps;
        private readonly int _ilOffset;
        internal ILDasmSwitchInstruction(OpCode opCode, int ilOffset, int[] jumps, int token, uint value, int size)
            :base(opCode,value,token,size)
        {
            _jumps = jumps;
            _ilOffset = ilOffset;
        }

        public int[] Jumps
        {
            get
            {
                return _jumps;
            }
        }

        public override void Dump(StringBuilder sb, bool showBytes = false)
        {
            if (showBytes)
            {
                sb.AppendFormat("{0,-4} | ", opCode.Value.ToString("X2"));
                sb.Append(string.Format("{0:X2}000000", (int)Value));
                for(int i = 0; i < Token; i++)
                {
                    sb.AppendLine();
                    sb.AppendFormat("{0,-14} | ", "");
                    sb.Append(string.Format("{0:X2}000000", _jumps[i]));
                }
                return;
            }
            sb.AppendFormat("{0,-10}", opCode);
            sb.Append("(");
            for(int i = 0; i < Token; i++)
            {
                sb.AppendLine();
                sb.AppendFormat("{0,-21}", "");
                sb.AppendFormat("IL_{0:x4},", (_ilOffset + Size + _jumps[i]));
            }
            sb.Length--; //Delete trailing ,
            sb.Append(")");
        }
    }
}
