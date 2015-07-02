using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing an assembly.
    /// </summary>
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

        /// <summary>
        /// Property that represent the Assembly name.
        /// </summary>
        public string Name
        {
            get
            {
                return GetCachedValue(_assemblyDefinition.Name, ref _name);
            }
        }

        /// <summary>
        /// Property containing the assembly culture. 
        /// </summary>
        public string Culture
        {
            get
            {
                return GetCachedValue(_assemblyDefinition.Culture, ref _culture);
            }
        }

        /// <summary>
        /// Property that represents the hash algorithm used on this assembly to hash the files.
        /// </summary>
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

        /// <summary>
        /// Version of the assembly.
        /// Containing:
        ///    MajorVersion
        ///    MinorVersion
        ///    BuildNumber
        ///    RevisionNumber
        /// </summary>
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

        /// <summary>
        /// A binary object representing a public encryption key for a strong-named assembly.
        /// Represented as a byte array on a string format (00 00 00 00 00 00 00 00 00)
        /// </summary>
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

        /// <summary>
        /// Assembly flags if it is strong named, whether the JIT tracking and optimization is enabled, and if the assembly can be retargeted at run time to a different assembly version.
        /// </summary>
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

        /// <summary>
        /// The type definitions contained on the current assembly.
        /// </summary>
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

        /// <summary>
        /// Method to get the hash algorithm formatted in the MSIL syntax.
        /// </summary>
        /// <returns>string representing the hash algorithm.</returns>
        public string GetFormattedHashAlgorithm()
        {
            return String.Format("0x{0:x8}", HashAlgorithm);
        }

        /// <summary>
        /// Method to get the version formatted to the MSIL syntax. MajorVersion:MinorVersion:BuildVersion:RevisionVersion.
        /// </summary>
        /// <returns>string representing the version.</returns>
        public string GetFormattedVersion()
        {
            int build = Version.Build;
            int revision = Version.Revision;
            if (build == -1) build = 0;
            if (revision == -1) revision = 0;
            return String.Format("{0}:{1}:{2}:{3}", Version.Major.ToString(), Version.Minor.ToString(), build.ToString(), revision.ToString());
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
        
    }
}
