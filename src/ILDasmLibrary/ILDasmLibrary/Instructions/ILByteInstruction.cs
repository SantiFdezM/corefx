using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    public class ILByteInstruction : ILNumericValueInstruction<byte>
    {
        internal ILByteInstruction(OpCode opCode, byte value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            return Value.ToString("X2");
        }
    }
}
