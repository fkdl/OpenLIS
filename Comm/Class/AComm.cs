using System.Windows.Forms;
using Comm.UI.MDI;

namespace Comm.Class
{
    public abstract class AComm
    {
        protected AComm()
        {
            // default values
            ModuleName = string.Empty;
            ModuleVersion = string.Empty;
            MdiChild = new MdiCommon();
        }

        #region Module Info
        public virtual string ModuleName { get; }
        public virtual string ModuleVersion { get; }
        public virtual MdiCommon MdiChild { get; }
        #endregion
        
    }

    /// <summary>
    /// Types of Comm
    /// </summary>
    public enum CommType
    {
        // cached data
        OleDb,
        
        // exported data
        Csv,
        Excel,
        PlainFile,

        // online data
        Serial,
        Network
    }
}
