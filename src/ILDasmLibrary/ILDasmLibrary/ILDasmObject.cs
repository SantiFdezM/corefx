using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public abstract class ILDasmObject
    {
        internal readonly Readers _readers;
        internal ILDasmObject(Readers readers)
        {
            _readers = readers;
        }

        protected string GetCachedValue(StringHandle value, ref string storage)
        {
            if(storage != null)
            {
                return storage;
            }
            storage = _readers.MdReader.GetString(value);
            return storage;
        }
    }
}
