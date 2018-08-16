using Base.Conf;

namespace Comm.Class.Comm.CachedData
{
    public static class OleDbConf
    {
        private static readonly string ConfigName;

        static OleDbConf()
        {
            ConfigName = "CommConf";
        }

        public static string ConnectionString
        {
            get { return BaseConf.ConfigStrR("conf/oledb/connection_string", ConfigName); }
            set { BaseConf.ConfigStrW("conf/oledb/connection_string", ConfigName, value); }
        }

        public static string SqlForSearch
        {
            get { return BaseConf.ConfigStrR("conf/oledb/sql_for_search", ConfigName); }
            set { BaseConf.ConfigStrW("conf/oledb/sql_for_search", ConfigName, value); }
        }

        public static bool AutoUpload
        {
            get { return BaseConf.ConfigBoolR("conf/oledb/auto_upload", ConfigName); }
            set { BaseConf.ConfigBoolW("conf/oledb/auto_upload", ConfigName, value); }
        }

        public static string SqlForMark
        {
            get { return BaseConf.ConfigStrR("conf/oledb/sql_for_mark", ConfigName); }
            set { BaseConf.ConfigStrW("conf/oledb/sql_for_mark", ConfigName, value); }
        }

        public static int AutoUploadIntervalSec
        {
            get { return BaseConf.ConfigIntR("conf/oledb/auto_upload_interval_sec", ConfigName); }
            set { BaseConf.ConfigIntW("conf/oledb/auto_upload_interval_sec", ConfigName, value); }
        }
    }
}