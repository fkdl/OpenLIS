namespace Base.Conf
{
    public static class SysConf
    {
        private static readonly string ConfigName;

        static SysConf()
        {
            ConfigName = "SysConf";
        }
        
        public static string SystemName => BaseConf.ConfigStr("conf/system/system_name", ConfigName);

        public static bool IsDebugMode => BaseConf.ConfigBool("/conf/debug/debug_mode", ConfigName);

        #region Database configs
        public static string DbAddress => BaseConf.ConfigStr("conf/db_conn/address", ConfigName);
        public static string DbUserName => BaseConf.ConfigStr("conf/db_conn/user_name", ConfigName);
        public static string DbPassword => BaseConf.ConfigStr("conf/db_conn/password", ConfigName);
        public static string DbDefaultDb => BaseConf.ConfigStr("conf/db_conn/default_db", ConfigName);
        public static string DbSslMode => BaseConf.ConfigStr("conf/db_conn/ssl_mode", ConfigName);

        #endregion

        #region SerialCom
        #endregion

        #region Format
        public static string DateTimeFormat => BaseConf.ConfigStr("conf/format/datetime_format", ConfigName);

        #endregion
    }


}
