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
                    Doc.Load($"./Configs/{configName}.xml");
                    _configName = configName;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on loading {configName}.xml. Exception message: {ex.Message}");
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
                Debug.Assert(node?.Attributes != null);
                var attr = node.Attributes["value"];
                var val = attr.Value;

                LogHelper.WriteLogInfo($"Value at xpath {nodePath} read: {val}");

                return val;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
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
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
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
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
                return false;
            }
        }
    }
}
