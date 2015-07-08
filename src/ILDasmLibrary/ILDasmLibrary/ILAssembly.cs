using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing an assembly.
    /// </summary>
    public class ILAssembly : ILObject
    {
        private AssemblyDefinition _assemblyDefinition;
        private string _publicKey;
        private IEnumerable<ILTypeDefinition> _typeDefinitions;
        private string _name;
        private string _culture;
        private int _hashAlgorithm;
        private Version _version;
        private IEnumerable<AssemblyReference> _assemblyReferences;
        private IEnumerable<CustomAttribute> _customAttribues;

        private ILAssembly(Readers readers) 
            : base(readers)
        {
            _hashAlgorithm = -1;
            _assemblyDefinition = _readers.MdReader.GetAssemblyDefinition();
        }

        #region Public APIs

        public static ILAssembly Create(Stream stream)
        {
            return new ILAssembly(Readers.Create(stream));
        }

        public static ILAssembly Create(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File doesn't exist in path");
            }

            return Create(File.OpenRead(path));
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
        /// Property containing the assembly culture. known as locale, such as en-US or fr-CA.
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
        public IEnumerable<ILTypeDefinition> TypeDefinitions
        {
            get
            {
                if (_typeDefinitions == null)
                {
                    _typeDefinitions = PopulateTypeDefinitions();
                }
                return _typeDefinitions.AsEnumerable<ILTypeDefinition>();
            }
        }

        public IEnumerable<AssemblyReference> AssemblyReferences
        {
            get
            {
                throw new NotImplementedException("AssemblyReferences for assembly");
            }
        }

        public IEnumerable<CustomAttribute> CustomAttributes
        {
            get
            {
                throw new NotImplementedException("Custom Attributes on assembly def");
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

        #endregion

        #region Private Methods

        private IEnumerable<ILTypeDefinition> PopulateTypeDefinitions()
        {
            var handles = _readers.MdReader.TypeDefinitions;
            foreach(var handle in handles)
            {
                if (handle.IsNil)
                {
                    continue;
                }
                var typeDefinition = _readers.MdReader.GetTypeDefinition(handle);
                if(typeDefinition.GetDeclaringType().IsNil)
                    yield return new ILTypeDefinition(typeDefinition, _readers, MetadataTokens.GetToken(handle));
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

        #endregion
    }
}
