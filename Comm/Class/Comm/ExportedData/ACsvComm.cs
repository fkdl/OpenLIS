using Comm.UI.MDI;
using Comm.UI.MDI.ExportedData;

namespace Comm.Class.Comm.ExportedData
{
    public abstract class ACsvComm<TMdiCsv> : AComm
        where TMdiCsv : MdiCsv, new()
    {
        protected ACsvComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiCsv();
        }

        public override string ModuleName { get; }

        public override string ModuleVersion { get; }

        public override MdiCommon MdiChild { get; }
    }
}
