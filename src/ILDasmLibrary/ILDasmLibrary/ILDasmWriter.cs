using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System.Collections.Immutable;
using System.Text;

namespace ILDasmLibrary
{
    public struct ILDasmWriter
    {
        private string indent;
        private int indentation;

        public ILDasmWriter()
        {
            indent = "  ";
            indentation = 0;
        }

        private void Indent()
        {
            indentation++;
        }

        private void Unindent()
        {
            indentation--;
        }

        private void WriteIndentation(StringBuilder sb)
        {
            for (int i = 0; i < indentation; i++)
            {
                sb.Append(indent);
            }
        }

        public string DumpMethod(ILDasmMethodDefinition _methodDefinition, bool showBytes = false)
        {
            StringBuilder sb = new StringBuilder();
            DumpMethodDefinition(_methodDefinition, sb);
            Indent();
            DumpMethodHeader(_methodDefinition, sb);
            DumpMethodBody(_methodDefinition, sb, showBytes);
            Unindent();
            WriteIndentation(sb);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void DumpMethodHeader(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            DumpCustomAttributes(_methodDefinition, sb);
            if(_methodDefinition.RelativeVirtualAdress == 0)
            {
                return;
            }
            if (_methodDefinition.IsEntryPoint)
            {
                WriteIndentation(sb);
                sb.AppendLine(".entrypoint");
            }
            WriteIndentation(sb);
            sb.AppendLine(string.Format("// Code size {0,8} (0x{0:x})", _methodDefinition.Size));
            WriteIndentation(sb);
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
            WriteIndentation(sb);
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
                    WriteIndentation(sb);
                    sb.AppendFormat("{0,13}", "");
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
            WriteIndentation(sb);
            sb.AppendLine(_methodDefinition.GetDecodedSignature());
            sb.AppendLine("{");
        }

        private void DumpMethodBody(ILDasmMethodDefinition _methodDefinition, StringBuilder sb, bool showBytes)
        {
            if (_methodDefinition.RelativeVirtualAdress == 0) return;
            int ilOffset = 0;
            int regionIndex = 0;
            var exceptionRegions = _methodDefinition.ExceptionRegions;
            var instructions = _methodDefinition.Instructions;
            for(int instructionIndex = 0;instructionIndex < instructions.Length;instructionIndex++)
            {
                var instruction = instructions[instructionIndex];
                if (regionIndex < exceptionRegions.Length)
                {
                    if (ilOffset == exceptionRegions[regionIndex].TryOffset)
                    {
                        WriteIndentation(sb);
                        sb.Append(".try");
                        sb.AppendLine("{");
                        Indent();
                        DumpInstruction(instruction, sb, ref ilOffset, showBytes);
                        instructionIndex++;
                        var region = exceptionRegions[regionIndex++];
                        while (ilOffset < region.TryOffset + region.TryLength)
                        {
                            DumpInstruction(instructions[instructionIndex++], sb, ref ilOffset, showBytes);
                        }
                        Unindent();
                        WriteIndentation(sb);
                        sb.AppendLine("} //end of try");
                        //while (ilOffset < region.HandlerOffset + region.HandlerLength)
                        //{
                            
                        //}
                        instructionIndex--;
                        continue;
                    }
                }
                DumpInstruction(instruction, sb, ref ilOffset, showBytes);
            }
        }

        private void DumpExceptionRegionsBeforeFinally(ImmutableArray<ILInstruction> instructions, StringBuilder sb, int finallyIndex, ref int ilOffset, ref int regionIndex)
        {

        }

        private void DumpInstruction(ILInstruction instruction, StringBuilder sb, ref int ilOffset, bool showBytes)
        {
            WriteIndentation(sb);
            sb.AppendFormat("IL_{0:x4}:", ilOffset);
            sb.Append(indent);
            instruction.Dump(sb, showBytes);
            ilOffset += instruction.Size;
            sb.AppendLine();
        }

        private void DumpCustomAttributes(ILDasmMethodDefinition _methodDefinition, StringBuilder sb)
        {
            foreach(var attribute in _methodDefinition.CustomAttributes)
            {
                var result = ILDasmDecoder.DecodeCustomAttribute(attribute, _methodDefinition);
                WriteIndentation(sb);
                sb.AppendFormat(".custom {0}", result);
                sb.AppendLine();
            }
        }
    }
}
