using System.Diagnostics;
using Base.Logger;
using System;
using System.Xml;

namespace Base.Conf
{
    public static class BaseConf
    {
        private static readonly XmlDocument Doc = new XmlDocument();
        private static string _configName = string.Empty;
        
        private static void LoadConfig(string configName)
        {
            try
            {
                if (_configName != configName)
                {
                    Doc.Load(string.Format("./Configs/{0}.xml", configName));
                    _configName = configName;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(string.Format("Error occured on loading {0}.xml. Exception message: {1}", configName, ex.Message));
            }
        }

        /// <summary>
        /// General configuration reader, returns with string type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string ConfigStr(string nodePath, string configName)
        {
            try
            {
                LoadConfig(configName);

                var node = Doc.SelectSingleNode(nodePath);
                Debug.Assert(node != null && node.Attributes != null);
                var attr = node.Attributes["value"];
                var val = attr.Value;

                LogHelper.WriteLogInfo(string.Format("Value at xpath {0} read: {1}", nodePath, val));

                return val;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(string.Format("Error occured on reading xpath {0}. Exception message: {1}", nodePath, ex.Message));
                return "";
            }
        }

        /// <summary>
        /// General configuration reader, returns with int type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static int ConfigInt(string nodePath, string configName)
        {
            var str = ConfigStr(nodePath, configName);
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(string.Format("Error occured on reading xpath {0}. Exception message: {1}", nodePath, ex.Message));
                return -1;
            }
        }

        /// <summary>
        /// General configuration reader, returns with bool type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static bool ConfigBool(string nodePath, string configName)
        {
            var str = ConfigStr(nodePath, configName);
            try
            {
                return Convert.ToBoolean(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(string.Format("Error occured on reading xpath {0}. Exception message: {1}", nodePath, ex.Message));
                return false;
            }
        }
    }
}
