using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    public class ILIntInstruction : ILNumericValueInstruction<int>
    {
        internal ILIntInstruction(OpCode opCode, int value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            return Value.ToString("X2");
        }
    }
}
