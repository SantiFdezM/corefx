using ILDasmLibrary.Decoder;
using ILDasmLibrary.Instructions;
using System;
using System.IO;
using System.Collections.Generic;

namespace ILDasmLibrary.Visitor
{
    public class ILVisitor : IVisitor
    {
        private readonly ILVisitorOptions _options;
        private readonly TextWriter _writer;
        private string _indent = "  ";
        private int _indentation = 0;

        public ILVisitor(ILVisitorOptions options, TextWriter writer)
        {
            _options = options;
            _writer = writer;
        }

        private void Indent()
        {
            _indentation++;
        }

        private void Unindent()
        {
            _indentation--;
        }

        private void WriteIndentation()
        {
            for(int i = 0; i < _indentation; i++)
            {
                _writer.Write(_indent);
            }
        }

        public TextWriter Writer
        {
            get
            {
                return _writer;
            }
        }

        public void Visit(ILAssembly assembly)
        {
            foreach(var type in assembly.TypeDefinitions)
            {
                type.Accept(this);
            }
        }

        public void Visit(ILTypeDefinition typeDefinition)
        {
            WriteIndentation();
            _writer.Write(".class ");
            if(_options.ShowBytes)
                _writer.Write("/* {0} */", typeDefinition.Token.ToString("X8"));
            _writer.Write(typeDefinition.Name);
            _writer.WriteLine();
            WriteIndentation();
            _writer.WriteLine("{");
            Indent();
            foreach(var nestedType in typeDefinition.NestedTypes)
            {
                nestedType.Accept(this);
            }
            foreach(var method in typeDefinition.MethodDefinitions)
            {
                method.Accept(this);
            }
            Unindent();
            WriteIndentation();
            _writer.Write("} ");
            _writer.WriteLine(string.Format("// end of class {0}", typeDefinition.FullName));
        }

        public void Visit(ILMethodDefinition methodDefinition)
        {
            WriteMethodDefinition(methodDefinition);
            Indent();
            WriteMethodHeader(methodDefinition);
            int ilOffset = 0;
            int instructionIndex = 0;
            int lastRegionIndex = 0;
            if (methodDefinition.RelativeVirtualAddress != 0)
                WriteMethodBody(methodDefinition, ILExceptionRegion.CreateRegions(methodDefinition.ExceptionRegions), ref instructionIndex, ilOffset, lastRegionIndex, out lastRegionIndex);
            Unindent();
            WriteIndentation();
            _writer.WriteLine("}");
            _writer.WriteLine();
        }

        public void Visit(ILLocal local)
        {
            _writer.Write(string.Format("{0} {1}", local.Type, local.Name));
        }

        public void Visit(ILFloatInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            if (float.IsNaN(instruction.Value))
            {
                var data = BitConverter.GetBytes(instruction.Value);
                _writer.Write("(");
                _writer.Write(BitConverter.ToString(data).Replace("-", " "));
                _writer.WriteLine(")");
                return;
            }
            _writer.Write(instruction.Value.ToString());
            if (instruction.Value % 10 == 0)
            {
                _writer.Write(".");
            }
            _writer.WriteLine();
        }

        public void Visit(ILIntInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X8"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value.ToString());
        }

        public void Visit(ILShortBranchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Value.ToString("X2"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("IL_{0:x4}", (instruction.Token + instruction.Value + instruction.Size)));
        }

        public void Visit(ILStringInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                string tokenValue = string.Format("({0}){1}", (instruction.Token >> 24).ToString("X2"), instruction.Token.ToString("X8").Substring(2));
                WriteBytes(tokenValue, instruction);
            }
            _writer.Write(string.Format("{0,-13}", instruction.opCode));
            if (instruction.Token >> 24 == 0x70)
            {
                if (instruction.IsPrintable)
                {
                    _writer.WriteLine(string.Format("\"{0}\"", instruction.Value));
                    return;
                }
                _writer.WriteLine(string.Format("{0}", instruction.Value));
                return;
            }
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(ILVariableInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X4"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(ILSwitchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                _writer.Write(string.Format("/* {0,-4} | ", instruction.opCode.Value.ToString("X2")));
                string value = string.Format("{0:X2}000000", (int)instruction.Value);
                _writer.Write(string.Format("{0,-16} */ ", value));
                _writer.Write(string.Format("{0,-10}", instruction.opCode));
                _writer.Write("(");
                for (int i = 0; i < instruction.Token; i++)
                {
                    _writer.WriteLine();
                    WriteIndentation();
                    _writer.Write(string.Format("{0,12} {1,-4} | ", "/*", ""));
                    value = string.Format("{0:X2}000000", instruction.Jumps[i]);
                    _writer.Write(string.Format("{0,-16} */ ", value));
                    _writer.Write(string.Format("{0,11}", " "));
                    _writer.Write(string.Format("IL_{0:x4}", (instruction.IlOffset + instruction.Size + instruction.Jumps[i])));
                    if (i < instruction.Token - 1)
                    {
                        _writer.Write(",");
                    }
                }
                _writer.WriteLine(")");
                return;
            }
            _writer.Write(string.Format("{0,-10}", instruction.opCode));
            _writer.Write("(");
            for (int i = 0; i < instruction.Token; i++)
            {
                _writer.WriteLine();
                WriteIndentation();
                _writer.Write(string.Format("{0,-21}", ""));
                _writer.Write(string.Format("IL_{0:x4}", (instruction.IlOffset + instruction.Size + instruction.Jumps[i])));
                if(i < instruction.Token - 1)
                {
                    _writer.Write(",");
                }
            }
            _writer.WriteLine(")");
        }

        public void Visit(ILShortVariableInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X2"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(ILLongInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("0x{0:x}", instruction.Value));
        }

        public void Visit(ILInstructionWithNoValue instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(string.Empty, instruction);
            }
            _writer.WriteLine(string.Format("{0}", instruction.opCode));
        }

        public void Visit(ILDoubleInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            if (double.IsNaN(instruction.Value))
            {
                var data = BitConverter.GetBytes(instruction.Value);
                _writer.Write("(");
                _writer.Write(BitConverter.ToString(data).Replace("-", " "));
                _writer.WriteLine(")");
                return;
            }
            _writer.Write(instruction.Value.ToString());
            if (instruction.Value % 10 == 0)
            {
                _writer.Write(".");
            }
            _writer.WriteLine();
        }

        public void Visit(ILByteInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X8"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value.ToString());
        }

        public void Visit(ILBranchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Value.ToString("X4"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("IL_{0:x4}", (instruction.Token + instruction.Value + instruction.Size)));
        }

        private void WriteMethodDefinition(ILMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.WriteLine(methodDefinition.GetDecodedSignature());
            WriteIndentation();
            _writer.WriteLine("{");
        }

        private void WriteMethodHeader(ILMethodDefinition methodDefinition)
        {
            WriteCustomAttributes(methodDefinition);
            if(methodDefinition.RelativeVirtualAddress == 0)
            {
                return;
            }

            if (methodDefinition.IsImplementation)
            {
                WriteOverridenMethod(methodDefinition);
            }

            if (methodDefinition.IsEntryPoint)
            {
                WriteIndentation();
                _writer.WriteLine(".entrypoint");
            }

            WriteIndentation();
            _writer.WriteLine(string.Format("// code size {0,8} (0x{0:x})", methodDefinition.Size));
            WriteIndentation();
            _writer.WriteLine(string.Format(".maxstack {0,2}", methodDefinition.MaxStack));

            if (methodDefinition.HasLocals)
            {
                WriteLocals(methodDefinition);
            }
        }

        private void WriteLocals(ILMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.Write(".locals");

            if (methodDefinition.LocalVariablesInitialized)
            {
                _writer.Write(" init");
            }

            int i = 0;
            var locals = methodDefinition.Locals;
            _writer.Write("(");
            foreach(var local in locals)
            {
                if(i > 0)
                {
                    WriteIndentation();
                    _writer.Write(string.Format("{0,13}", ""));
                }
                _writer.Write(string.Format("[{0}] ", i));
                local.Accept(this);
                if(i < locals.Length - 1)
                {
                    _writer.WriteLine(",");
                }
                i++;
            }
            _writer.WriteLine(")");
        }

        private void WriteOverridenMethod(ILMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.Write(".override ");
            int token = methodDefinition.MethodDeclarationToken;
            if (ILDecoder.IsMemberReference(token))
            {
                _writer.Write("method ");
                _writer.Write(ILDecoder.SolveMethodName(methodDefinition._readers.MdReader, token, methodDefinition.Provider));
                if (_options.ShowBytes)
                    _writer.Write(string.Format(" /* {0} */", token.ToString("X8")));
                _writer.WriteLine();
                return;
            }
            _writer.Write(ILDecoder.DecodeOverridenMethodName(methodDefinition._readers.MdReader, token, methodDefinition.Provider));
            if (_options.ShowBytes)
                _writer.Write(string.Format(" /* {0} */", token.ToString("X8")));
            _writer.WriteLine();
        }

        private void WriteCustomAttributes(ILMethodDefinition methodDefinition)
        {
            foreach(var attribute in methodDefinition.CustomAttributes)
            {
                var result = ILDecoder.DecodeCustomAttribute(attribute, methodDefinition);
                WriteIndentation();
                _writer.WriteLine(string.Format(".custom {0}", result));
            }
        }

        private int WriteMethodBody(ILMethodDefinition methodDefinition, IReadOnlyList<ILExceptionRegion> exceptionRegions, ref int instructionIndex, int ilOffset, int regionIndex, out int nextRegionIndex)
        {
            int lastRegionIndex = regionIndex - 1;
            var instructions = methodDefinition.Instructions;
            for (; instructionIndex < instructions.Length; instructionIndex++)
            {
                var instruction = instructions[instructionIndex];
                if (EndFilterRegion(exceptionRegions, lastRegionIndex, ilOffset))
                {
                    Unindent();
                    WriteIndentation();
                    _writer.WriteLine("} // end filter");
                    WriteIndentation();
                    _writer.WriteLine("{ // handler");
                    Indent();
                }

                if (StartRegion(exceptionRegions, regionIndex, ilOffset))
                {
                    var region = exceptionRegions[regionIndex];
                    WriteIndentation();
                    _writer.WriteLine(region.ToString(methodDefinition.Provider));
                    WriteIndentation();
                    _writer.WriteLine("{");
                    Indent();
                    ilOffset = WriteMethodBody(methodDefinition, exceptionRegions, ref instructionIndex, ilOffset, regionIndex + 1, out regionIndex);
                    Unindent();
                    WriteIndentation();
                    _writer.Write("}");
                    _writer.WriteLine(string.Format(" // end {0}", (region.Kind == HandlerKind.Try ? ".try" : "handler")));
                }

                else
                {
                    WriteIndentation();
                    _writer.Write(string.Format("IL_{0:x4}:", ilOffset));
                    _writer.Write(_indent);
                    instruction.Accept(this);
                    ilOffset += instruction.Size;
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

        private void WriteBytes(string bytes, ILInstruction instruction)
        {
            _writer.Write(string.Format("/* {0,-4} | ", instruction.opCode.Value.ToString("X2")));
            _writer.Write(string.Format("{0,-16} */ ", bytes));
        }
    }
}
