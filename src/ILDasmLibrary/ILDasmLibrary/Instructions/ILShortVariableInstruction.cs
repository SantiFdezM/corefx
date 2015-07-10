using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILShortVariableInstruction : ILInstructionWithValue<string>, IVisitable
    {
        internal ILShortVariableInstruction(OpCode opCode, string name, int token, int size) 
            : base(opCode, name, token, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
