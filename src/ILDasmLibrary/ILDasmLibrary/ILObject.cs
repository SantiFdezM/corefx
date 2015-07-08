using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    public abstract class ILObject
    {
        internal readonly Readers _readers;
        internal ILObject(Readers readers)
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
