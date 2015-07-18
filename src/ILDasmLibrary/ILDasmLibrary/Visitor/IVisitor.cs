using ILDasmLibrary.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary.Visitor
{
    public interface IVisitor
    {
        void Visit(ILAssembly assembly);
        void Visit(ILTypeDefinition typeDefinition);
        void Visit(ILMethodDefinition methodDefinition);
        void Visit(ILLocal local);
        void Visit(ILCustomAttribute attribute);
        void Visit(ILProperty property);
        void Visit(ILEventDefinition eventDef);
        void Visit(ILBranchInstruction branchInstruction);
        void Visit(ILByteInstruction byteInstruction);
        void Visit(ILDoubleInstruction doubleInstruction);
        void Visit(ILSingleInstruction floatInstruction);
        void Visit(ILInstructionWithNoValue instruction);
        void Visit(ILIntInstruction intInstruction);
        void Visit(ILInt64Instruction longInstruction);
        void Visit(ILShortBranchInstruction shortBranchInstruction);
        void Visit(ILShortVariableInstruction shortVariableInstruction);
        void Visit(ILStringInstruction stringInstruction);
        void Visit(ILSwitchInstruction switchInstruction);
        void Visit(ILVariableInstruction variableInstruction);
    }
}
