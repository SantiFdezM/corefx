using ILDasmLibrary.Instructions;
using System;
using System.Text;

namespace ILDasmLibrary
{
    public struct ILDasmWriter
    {
        private string indent;

        public ILDasmWriter()
        {
            indent = "  ";
        }
        public string DumpMethod(ILDasmMethodDefinition _methodDefinition, bool showBytes = false)
        {
            StringBuilder sb = new StringBuilder();
            DumpMethodDefinition(_methodDefinition, sb);
            DumpMethodHeader(_methodDefinition, sb);
            DumpMethodBody(_methodDefinition, sb, showBytes);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void DumpMethodHeader(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            
        }

        private void DumpMethodDefinition(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            sb.AppendLine(_methodDefinition.GetDecodedSignature());
            sb.AppendLine("{");
        }

        private void DumpMethodBody(ILDasmMethodDefinition _methodDefinition, StringBuilder sb, bool showBytes)
        {
            int ilOffset = 0;
            foreach(ILInstruction instruction in _methodDefinition.Instructions)
            {
                sb.AppendFormat("IL_{0:x4}:", ilOffset);
                sb.Append(indent);
                instruction.Dump(sb, showBytes);
                ilOffset += instruction.Size;
                sb.AppendLine();
            }
        }
    }
}
