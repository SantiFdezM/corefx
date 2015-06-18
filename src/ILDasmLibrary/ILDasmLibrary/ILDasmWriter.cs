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
            if(_methodDefinition.RelativeVirtualAdress != 0)
            {
                DumpMethodHeader(_methodDefinition, sb);
                DumpMethodBody(_methodDefinition, sb, showBytes);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void DumpMethodHeader(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            if (_methodDefinition.IsEntryPoint)
            {
                sb.AppendLine(".entrypoint");
            }
            sb.AppendLine(string.Format("// Code size {0,8} (0x{0:x})", _methodDefinition.Size));
            sb.AppendFormat(".maxstack {0,2}", _methodDefinition.MaxStack);
            if (_methodDefinition.HasLocals)
            {
                sb.AppendLine();
                DumpLocals(_methodDefinition, sb);
            }
            sb.AppendLine();
        }

        private void DumpLocals(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            sb.Append(".locals");
            if (_methodDefinition.LocalVariablesInitialized)
            {
                sb.Append(" init");
            }
            int i = 0;
            var locals = _methodDefinition.Locals;
            sb.Append("(");
            foreach(var local in locals)
            {
                if(i > 0)
                {
                    sb.AppendFormat("{0,14}", "");
                }
                sb.AppendFormat("[{0}] {1} {2}", i, local.Type, local.Name);
                sb.AppendLine(",");
                i++;
            }
            sb.Length-=3; //remove trailing \n,;
            sb.Append(")");
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
