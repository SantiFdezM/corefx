using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public class ILDasmReaders
    {
        internal readonly Readers _readers;
        internal ILDasmReaders(Readers readers)
        {
            _readers = readers;
        }
    }
}
