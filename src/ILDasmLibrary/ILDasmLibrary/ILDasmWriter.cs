using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;
using ILDasmLibrary.Instructions;
using ILDasmLibrary.Decoder;
using System.Reflection.Metadata.Decoding;

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
            DumpMethodBody(_methodDefinition, sb, showBytes);
            sb.AppendLine("}");
            return sb.ToString();
        }
        
        private void DumpMethodDefinition(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            sb.AppendLine(String.Format(".method {0}", _methodDefinition.Name));
            Console.WriteLine(_methodDefinition.Name);
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
