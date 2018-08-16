using Comm.UI.MDI;
using Comm.UI.MDI.ExportedData;

namespace Comm.Class.Comm.ExportedData
{
    public abstract class AExcelComm<TMdiExcel> : AComm
        where TMdiExcel : MdiExcel, new()
    {
        protected AExcelComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiExcel();
        }

        public override string ModuleName { get; }

        public override string ModuleVersion { get; }

        public override MdiCommon MdiChild { get; }
    }
}