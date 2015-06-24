﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ILDasmLibrary.Intructions
{
    public abstract class ILDasmInstruction
    {
        private OpCode _opCode;
        private int _size;

        internal ILDasmInstruction(OpCode opCode, int size)
        {
            _opCode = opCode;
            _size = size;
        }

        public OpCode opCode
        {
            get
            {
                return _opCode;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }

        abstract public void Dump(StringBuilder sb, bool showBytes = false);
    }
}