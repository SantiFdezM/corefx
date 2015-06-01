using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary
{
    internal class ILWriterHelpers
    {
        internal static ILWriterHelpers Instance = new ILWriterHelpers();

        internal readonly OpCode[] _oneByteOpCodes;
        internal readonly OpCode[] _twoByteOpCodes;
            
        private ILWriterHelpers()
        {
            _oneByteOpCodes = new OpCode[0x100];
            _twoByteOpCodes = new OpCode[0x100];

            var opCodeType = typeof(OpCode);
            var fields = typeof(OpCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach(var field in fields)
            {
                if (field.FieldType != opCodeType) continue;

                OpCode opCode = (OpCode)field.GetValue(null);
                var value = unchecked((ushort)opCode.Value);
                if(value < 0x100)
                {
                    _oneByteOpCodes[value] = opCode;
                }
                else if((value & 0xff00) == 0xfe00)
                {
                    _twoByteOpCodes[value & 0xff] = opCode;
                }
            }
        }
    }
}
