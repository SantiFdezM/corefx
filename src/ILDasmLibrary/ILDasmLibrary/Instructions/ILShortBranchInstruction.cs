using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILShortBranchInstruction : ILInstructionWithValue<sbyte>, IVisitable
    {
        internal ILShortBranchInstruction(OpCode opCode, sbyte value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        
    }
}
