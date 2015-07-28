using ILDasmLibrary.Decoder;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILDasmLibrary
{
    public struct ILMethodImport
    {
        private Readers _readers;
        private string _name;
        private MethodImport _methodImport;
        private ILModuleReference _moduleReference;
        private bool _isModuleReferenceInitialized;
        private MethodImportAttributes _attributes;

        internal static ILMethodImport Create(MethodImport methodImport, ref Readers readers)
        {
            ILMethodImport import = new ILMethodImport();
            import._methodImport = methodImport;
            import._readers = readers;
            import._isModuleReferenceInitialized = false;
            return import;
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_methodImport.Name, _readers, ref _name);
            }
        }

        public bool IsNil
        {
            get
            {
                return _methodImport.Module.IsNil;
            }
        }

        public ILModuleReference ModuleReference
        {
            get
            {
                if (!_isModuleReferenceInitialized)
                {
                    if (IsNil)
                    {
                        throw new InvalidOperationException("Method Import is nil");
                    }
                    _isModuleReferenceInitialized = true;
                    ModuleReferenceHandle handle = _methodImport.Module;
                    _moduleReference = ILModuleReference.Create(_readers.MdReader.GetModuleReference(handle), ref _readers, MetadataTokens.GetToken(handle));
                }
                return _moduleReference;
            }
        }

        public MethodImportAttributes Attributes
        {
            get
            {
                return _methodImport.Attributes;
            }
        }

        public string GetFlags()
        {
           return string.Format("{0}{1}");
        }
    }
}
