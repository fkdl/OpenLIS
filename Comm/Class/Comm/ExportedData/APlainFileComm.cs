using Comm.UI.MDI;
using Comm.UI.MDI.ExportedData;

namespace Comm.Class.Comm.ExportedData
{
    public abstract class APlainFileComm<TMdiPlainFile> : AComm
        where TMdiPlainFile : MdiPlainFile, new()
    {
        protected APlainFileComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiPlainFile();
        }

        public override string ModuleName { get; }

        public override string ModuleVersion { get; }

        public override MdiCommon MdiChild { get; }
    }
}