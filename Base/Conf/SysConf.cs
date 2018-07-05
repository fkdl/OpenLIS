namespace Base.Conf
{
    public static class SysConf
    {
        private static readonly string ConfigName = "";

        static SysConf()
        {
            ConfigName = "SysConf";
        }
        
        public static string SystemName { get { return BaseConf.ConfigStr("conf/system/system_name", ConfigName); } }
        
        public static bool IsDebugMode { get { return BaseConf.ConfigBool("/conf/debug/debug_mode", ConfigName); } }

        #region Database configs
        public static string DbAddress { get { return BaseConf.ConfigStr("conf/db_conn/address", ConfigName); } }
        public static string DbUserName { get { return BaseConf.ConfigStr("conf/db_conn/user_name", ConfigName); } }
        public static string DbPassword { get { return BaseConf.ConfigStr("conf/db_conn/password", ConfigName); } }
        public static string DbDefaultDb { get { return BaseConf.ConfigStr("conf/db_conn/default_db", ConfigName); } }
        #endregion

        #region SerialCom
        #endregion

        #region Format
        public static string DateTimeFormat { get { return BaseConf.ConfigStr("conf/format/datetime_format", ConfigName); } }
        #endregion
    }


}
