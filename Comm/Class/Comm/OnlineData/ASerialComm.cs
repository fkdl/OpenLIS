using Comm.UI.MDI;
using Comm.UI.MDI.OnlineData;

namespace Comm.Class.Comm.OnlineData
{
    public abstract class ASerialComm<TMdiSerial> : AComm
        where TMdiSerial : MdiSerial, new()
    {
        protected ASerialComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiSerial();
        }

        public override string ModuleName { get; }

        public override string ModuleVersion { get; }

        public override MdiCommon MdiChild { get; }
    }
}