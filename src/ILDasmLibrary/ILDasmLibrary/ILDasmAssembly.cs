using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ILDasmLibrary
{
    public class ILDasmAssembly : ILDasmObject
    {
        private AssemblyDefinition _assemblyDefinition;
        private string _publicKey;
        private IList<ILDasmTypeDefinition> _typeDefinitions;
        private string _name;
        private string _culture;
        private int _hashAlgorithm;
        private Version _version;

        internal ILDasmAssembly(AssemblyDefinition assemblyDef, Readers readers) 
            : base(readers)
        {
            _hashAlgorithm = -1;
            _assemblyDefinition = assemblyDef;
        }

        public string Name
        {
            get
            {
                return GetCachedValue(_assemblyDefinition.Name, ref _name);
            }
        }

        public string Culture
        {
            get
            {
                return GetCachedValue(_assemblyDefinition.Name, ref _culture);
            }
        }

        public int HashAlgorithm
        {
            get
            {
                if(_hashAlgorithm == -1)
                {
                    _hashAlgorithm = Convert.ToInt32(_assemblyDefinition.HashAlgorithm);
                }
                return _hashAlgorithm;
            }
        }

        public Version Version
        {
            get
            {
                if(_version == null)
                {
                    _version = _assemblyDefinition.Version;
                }
                return _version;
            }
        }

        public string PublicKey
        {
            get
            {
                if (_publicKey == null)
                {
                    _publicKey = GetPublicKey();
                }
                return _publicKey;
            }
        }

        public string Flags
        {
            get
            {
                if (_assemblyDefinition.Flags.HasFlag(System.Reflection.AssemblyFlags.Retargetable))
                {
                    return "retargetable";
                }
                return string.Empty;
            }
        }

        public IEnumerable<ILDasmTypeDefinition> TypeDefinitions
        {
            get
            {
                if (_typeDefinitions == null)
                {
                    PopulateTypeDefinitions();
                }
                return _typeDefinitions.AsEnumerable<ILDasmTypeDefinition>();
            }
        }

        private void PopulateTypeDefinitions()
        {
            var handles = _readers.MdReader.TypeDefinitions;
            _typeDefinitions = new List<ILDasmTypeDefinition>();
            foreach(var handle in handles)
            {
                if (handle.IsNil)
                {
                    continue;
                }
                var typeDefinition = _readers.MdReader.GetTypeDefinition(handle);
                _typeDefinitions.Add(new ILDasmTypeDefinition(typeDefinition, _readers));
            }
            
        }

        private string GetPublicKey()
        {
            var bytes = _readers.MdReader.GetBlobBytes(_assemblyDefinition.PublicKey);
            if (bytes.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            foreach (byte _byte in bytes)
            {
                sb.Append(String.Format("{0:x2}",_byte));                  
                sb.Append(" ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public string GetFormattedHashAlgorithm()
        {
            return String.Format("0x{0:x8}", HashAlgorithm);
        }

        public string GetFormattedVersion()
        {
            int build = Version.Build;
            int revision = Version.Revision;
            if (build == -1) build = 0;
            if (revision == -1) revision = 0;
            return String.Format("{0}:{1}:{2}:{3}", Version.Major.ToString(), Version.Minor.ToString(), build.ToString(), revision.ToString());
        }
    }
}
