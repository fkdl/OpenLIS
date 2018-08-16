using Comm.UI.MDI;
using Comm.UI.MDI.OnlineData;

namespace Comm.Class.Comm.OnlineData
{
    public abstract class AEthernetComm<TMdiEthernet> : AComm
        where TMdiEthernet : MdiEthernet, new()
    {
        protected AEthernetComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiEthernet();
        }

        public override string ModuleName { get; }

        public override string ModuleVersion { get; }

        public override MdiCommon MdiChild { get; }
    }
}