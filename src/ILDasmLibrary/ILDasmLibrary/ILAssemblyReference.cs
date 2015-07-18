using ILDasmLibrary.Decoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public struct ILAssemblyReference
    {
        private Readers _readers;
        private AssemblyReference _assemblyRef;
        private string _culture;
        private string _name;
        private Version _version;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private byte[] _hashValue;
        private byte[] _publicKeyOrToken;

        internal ILAssemblyReference Create(AssemblyReference assemblyRef, ref Readers readers)
        {
            ILAssemblyReference assembly = new ILAssemblyReference();
            assembly._assemblyRef = assemblyRef;
            assembly._readers = readers;
            return assembly;
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_assemblyRef.Name, _readers, ref _name);
            }
        }

        public string Culture
        {
            get
            {
                return ILDecoder.GetCachedValue(_assemblyRef.Culture, _readers, ref _culture);
            }
        }

        public Version Version
        {
            get
            {
                if (_version == null)
                {
                    _version = _assemblyRef.Version;
                }
                return _version;
            }
        }

        public IEnumerable<ILCustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttributes == null)
                {
                    _customAttributes = GetCustomAttributes();
                }
                return _customAttributes;
            }
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _assemblyRef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
