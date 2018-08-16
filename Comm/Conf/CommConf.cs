using Base.Conf;

namespace Comm.Conf
{
    public static class CommConf
    {
        static CommConf()
        {
            const string configName = "CommConf";

            FormText = BaseConf.ConfigStrR("conf/ui/form_text", configName);
            TrayText = BaseConf.ConfigStrR("conf/ui/tray_text", configName);
            BallonTitle = BaseConf.ConfigStrR("conf/ui/ballon_title", configName);
            BallonText = BaseConf.ConfigStrR("conf/ui/ballon_text", configName);
            HideOnLoad = BaseConf.ConfigBoolR("conf/ui/hide_on_load", configName);

            DllFileName = BaseConf.ConfigStrR("conf/model/dll_file_name", configName);
            NameSpace = BaseConf.ConfigStrR("conf/model/name_space", configName);
            ClassName = BaseConf.ConfigStrR("conf/model/class_name", configName);
        }

        public static readonly string FormText;
        public static readonly string TrayText;
        public static readonly string BallonTitle;
        public static readonly string BallonText;
        public static readonly bool HideOnLoad;

        public static readonly string DllFileName;
        public static readonly string NameSpace;
        public static readonly string ClassName;

    }
}
