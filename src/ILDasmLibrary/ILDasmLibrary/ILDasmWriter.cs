using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Text;
using System;
using System.Collections.Generic;

namespace ILDasmLibrary
{
    public struct ILDasmWriter
    {
        private string _indent;
        private int _indentation;

        public ILDasmWriter(string indent = "  ", int indentation = 0)
        {
            _indent = indent;
            _indentation = indentation;
        }

        private void Indent()
        {
            _indentation++;
        }

        private void Unindent()
        {
            _indentation--;
        }

        private void WriteIndentation(StringBuilder sb)
        {
            for (int i = 0; i < _indentation; i++)
            {
                sb.Append(_indent);
            }
        }

        public string DumpMethod(ILDasmMethodDefinition _methodDefinition, bool showBytes = false)
        {
            StringBuilder sb = new StringBuilder();
            DumpMethodDefinition(_methodDefinition, sb);
            Indent();
            DumpMethodHeader(_methodDefinition, sb);
            int ilOffset = 0;
            int instructionIndex = 0;
            int lastRegionIndex = 0;
            if(_methodDefinition.RelativeVirtualAdress != 0)
                DumpMethodBody(_methodDefinition, ILDasmExceptionRegion.CreateRegions(_methodDefinition.ExceptionRegions) ,sb,ref instructionIndex, ilOffset,lastRegionIndex, showBytes, out lastRegionIndex);
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

        private int DumpMethodBody(ILDasmMethodDefinition _methodDefinition, IReadOnlyList<ILDasmExceptionRegion> exceptionRegions, StringBuilder sb, ref int instructionIndex, int ilOffset, int regionIndex, bool showBytes, out int nextRegionIndex)
        {
            int lastRegionIndex = regionIndex-1;
            var instructions = _methodDefinition.Instructions;
            for(;instructionIndex < instructions.Length;instructionIndex++)
            {
                var instruction = instructions[instructionIndex];
                if(EndFilterRegion(exceptionRegions, lastRegionIndex, ilOffset))
                {
                    Unindent();
                    WriteIndentation(sb);
                    sb.AppendLine("} // end filter");
                    WriteIndentation(sb);
                    sb.AppendLine("{ // handler");
                    Indent();
                }
                if(StartRegion(exceptionRegions, regionIndex, ilOffset))
                {
                    var region = exceptionRegions[regionIndex];
                    WriteIndentation(sb);
                    sb.AppendLine(region.ToString(_methodDefinition.Provider));
                    WriteIndentation(sb);
                    sb.AppendLine("{");
                    Indent();
                    ilOffset = DumpMethodBody(_methodDefinition, exceptionRegions, sb, ref instructionIndex, ilOffset, regionIndex + 1, showBytes, out regionIndex);
                    Unindent();
                    WriteIndentation(sb);
                    sb.Append("}");
                    sb.AppendLine(string.Format(" // end {0}", (region.Kind == HandlerKind.Try ? ".try":"handler")));
                }
                else
                {
                    DumpInstruction(instruction, sb, ref ilOffset, showBytes);
                }
                if (EndRegion(exceptionRegions, lastRegionIndex, ilOffset))
                {
                    break;
                }
            }
            nextRegionIndex = regionIndex;
            return ilOffset;
        }

        private static bool EndFilterRegion(IReadOnlyList<ILDasmExceptionRegion> exceptionRegions, int lastRegionIndex, int ilOffset)
        {
            return lastRegionIndex >= 0 && exceptionRegions[lastRegionIndex].Kind == HandlerKind.Filter && exceptionRegions[lastRegionIndex].StartOffset == ilOffset;
        }

        private static bool EndRegion(IReadOnlyList<ILDasmExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex >= 0 && exceptionRegions[regionIndex].EndOffset == ilOffset;
        }

        private static bool StartRegion(IReadOnlyList<ILDasmExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex < exceptionRegions.Count && 
                (exceptionRegions[regionIndex].StartOffset == ilOffset || exceptionRegions[regionIndex].FilterHandlerStart == ilOffset);
        }

        private void DumpInstruction(ILInstruction instruction, StringBuilder sb, ref int ilOffset, bool showBytes)
        {
            WriteIndentation(sb);
            sb.AppendFormat("IL_{0:x4}:", ilOffset);
            sb.Append(_indent);
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
