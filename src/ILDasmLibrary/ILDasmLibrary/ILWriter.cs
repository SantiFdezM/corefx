using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace ILDasmLibrary
{
    /// <summary>
    /// Struct containing public APIs to dump assembly members.
    /// </summary>
    public struct ILWriter
    {
        private static string _indent;
        private static int _indentation;

        public ILWriter(string indent = "  ", int indentation = 0)
        {
            _indent = indent;
            _indentation = indentation;
        }

        #region Public APIs

        /// <summary>
        /// Method that dumps the whole assembly to a file.
        /// </summary>
        /// <param name="_assembly">assembly to dump.</param>
        /// <param name="file">StreamWriter of the file where the assembly is intended to be dumped on.</param>
        public void DumpAssembly(ILAssembly _assembly, StreamWriter file)
        {
            if(file == null)
            {
                throw new ArgumentNullException("The stream writer can't be null");
            }
            // TO DO: Dump whole assembly to file.
        }

        /// <summary>
        /// Method that dumps the type definition as a string. Including all it's properties, fields, custom attributes, methods and nested types.
        /// </summary>
        /// <param name="_typeDefinition">The type that is intended to dump.</param>
        /// <param name="showBytes">Boolean parameter to show the bytes and tokens or not.</param>
        /// <returns>string representing the type</returns>
        public string DumpType(ILTypeDefinition _typeDefinition, bool showBytes = false)
        {
            // TO DO: Dump fields, dump custom attributes.
            StringBuilder sb = new StringBuilder();
            WriteIndentation(sb);
            sb.Append(".class ");
            sb.Append(string.Format("/* {0} */ ", _typeDefinition.Token.ToString("X8")));
            sb.Append(_typeDefinition.Name);
            sb.AppendLine();
            WriteIndentation(sb);
            sb.AppendLine("{");
            Indent();
            foreach (var nestedType in _typeDefinition.NestedTypes)
            {
                sb.Append(DumpType(nestedType, showBytes));
            }
            foreach(var method in _typeDefinition.MethodDefinitions)
            {
                sb.Append(DumpMethod(method, showBytes));
            }
            Unindent();
            WriteIndentation(sb);
            sb.Append("} ");
            sb.AppendLine(string.Format("// end of class {0}", _typeDefinition.FullName));
            return sb.ToString();
        }

        /// <summary>
        /// Method that dumps the method body representation as a string. (Signature, header, body).
        /// </summary>
        /// <param name="_methodDefinition">The method definition from the method that is intended to dump.</param>
        /// <param name="showBytes">Boolean parameter to show the bytes and tokens from instructions or not.</param>
        /// <returns>string representing the method definition.</returns>
        public string DumpMethod(ILMethodDefinition _methodDefinition, bool showBytes = false)
        {
            StringBuilder sb = new StringBuilder();
            DumpMethodDefinition(_methodDefinition, sb);
            Indent();
            DumpMethodHeader(_methodDefinition, sb);
            int ilOffset = 0;
            int instructionIndex = 0;
            int lastRegionIndex = 0;
            if (_methodDefinition.RelativeVirtualAddress != 0)
                DumpMethodBody(_methodDefinition, ILExceptionRegion.CreateRegions(_methodDefinition.ExceptionRegions), sb, ref instructionIndex, ilOffset, lastRegionIndex, showBytes, out lastRegionIndex);
            Unindent();
            WriteIndentation(sb);
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region Private helper methods
        private static void Indent()
        {
            _indentation++;
        }

        private static void Unindent()
        {
            _indentation--;
        }

        private static void WriteIndentation(StringBuilder sb)
        {
            for (int i = 0; i < _indentation; i++)
            {
                sb.Append(_indent);
            }
        }

        private static void DumpMethodHeader(ILMethodDefinition _methodDefinition, StringBuilder sb)
        {
            DumpCustomAttributes(_methodDefinition, sb);
            if(_methodDefinition.RelativeVirtualAddress == 0)
            {
                return;
            }
            if (_methodDefinition.IsImplementation)
            {
                DumpOverridenMethod(_methodDefinition, sb);
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

        private static void DumpOverridenMethod(ILMethodDefinition _methodDefinition, StringBuilder sb)
        {
            WriteIndentation(sb);
            sb.Append(".override ");
            int token = _methodDefinition.MethodDeclarationToken;
            if (ILDecoder.IsMemberReference(token))
            {
                sb.Append("method ");
                sb.Append(ILDecoder.SolveMethodName(_methodDefinition._readers.MdReader, token, _methodDefinition.Provider));
                sb.Append(string.Format(" /* {0} */", token.ToString("X8")));
                sb.AppendLine();
                return;
            }
            sb.Append(ILDecoder.DecodeOverridenMethodName(_methodDefinition._readers.MdReader, token, _methodDefinition.Provider));
            sb.Append(string.Format(" /* {0} */", token.ToString("X8")));
            sb.AppendLine();
        }

        private static void DumpLocals(ILMethodDefinition _methodDefinition, StringBuilder sb)
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

        private static void DumpMethodDefinition(ILMethodDefinition _methodDefinition, StringBuilder sb)
        {
            WriteIndentation(sb);
            sb.AppendLine(_methodDefinition.GetDecodedSignature());
            WriteIndentation(sb);
            sb.AppendLine("{");
        }

        private static int DumpMethodBody(ILMethodDefinition _methodDefinition, IReadOnlyList<ILExceptionRegion> exceptionRegions, StringBuilder sb, ref int instructionIndex, int ilOffset, int regionIndex, bool showBytes, out int nextRegionIndex)
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

        private static bool EndFilterRegion(IReadOnlyList<ILExceptionRegion> exceptionRegions, int lastRegionIndex, int ilOffset)
        {
            return lastRegionIndex >= 0 && exceptionRegions[lastRegionIndex].Kind == HandlerKind.Filter && exceptionRegions[lastRegionIndex].StartOffset == ilOffset;
        }

        private static bool EndRegion(IReadOnlyList<ILExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex >= 0 && exceptionRegions[regionIndex].EndOffset == ilOffset;
        }

        private static bool StartRegion(IReadOnlyList<ILExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex < exceptionRegions.Count && 
                (exceptionRegions[regionIndex].StartOffset == ilOffset || exceptionRegions[regionIndex].FilterHandlerStart == ilOffset);
        }

        private static void DumpInstruction(ILInstruction instruction, StringBuilder sb, ref int ilOffset, bool showBytes)
        {
            WriteIndentation(sb);
            sb.AppendFormat("IL_{0:x4}:", ilOffset);
            sb.Append(_indent);
            instruction.Dump(sb, showBytes);
            ilOffset += instruction.Size;
            sb.AppendLine();
        }

        private static void DumpCustomAttributes(ILMethodDefinition _methodDefinition, StringBuilder sb)
        {
            foreach(var attribute in _methodDefinition.CustomAttributes)
            {
                var result = ILDecoder.DecodeCustomAttribute(attribute, _methodDefinition);
                WriteIndentation(sb);
                sb.AppendFormat(".custom {0}", result);
                sb.AppendLine();
            }
        }

        #endregion
    }
}
