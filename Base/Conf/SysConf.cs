using System.Globalization;

namespace Base.Conf
{
    public static class SysConf
    {
        static SysConf()
        {
            const string configName = "SysConf";

            SystemName = BaseConf.ConfigStrR("conf/system/name", configName);
            SystemVersion = BaseConf.ConfigStrR("conf/system/version", configName);
            IsDebugMode = BaseConf.ConfigBoolR("/conf/debug/debug_mode", configName);

            DbAddress = BaseConf.ConfigStrR("conf/db_conn/address", configName);
            DbUserName = BaseConf.ConfigStrR("conf/db_conn/user_name", configName);
            DbPassword = BaseConf.ConfigStrR("conf/db_conn/password", configName);
            DbDefaultDb = BaseConf.ConfigStrR("conf/db_conn/default_db", configName);
            DbSslMode = BaseConf.ConfigStrR("conf/db_conn/ssl_mode", configName);

            DateTimeFormat = BaseConf.ConfigStrR("conf/format/datetime", configName);
            DateFormat = BaseConf.ConfigStrR("conf/format/date", configName);
            TimeFormat = BaseConf.ConfigStrR("conf/format/time", configName);
        }

        #region System Info
        public static readonly string SystemName;
        public static readonly string SystemVersion;
        public static readonly bool IsDebugMode;
        #endregion

        #region Database configs
        public static readonly string DbAddress;
        public static readonly string DbUserName;
        public static readonly string DbPassword;
        public static readonly string DbDefaultDb;
        public static readonly string DbSslMode;
        #endregion

        #region Format
        public static readonly string DateTimeFormat;
        public static readonly string DateFormat;
        public static readonly string TimeFormat;
        #endregion
    }
}
