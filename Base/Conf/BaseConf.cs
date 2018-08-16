using Base.Logger;
using System;
using System.Globalization;
using System.Xml;

namespace Base.Conf
{
    public static class BaseConf
    {
        private static string _docFile = string.Empty;
        private static readonly XmlDocument Doc = new XmlDocument();
        private static string _configName = string.Empty;

        private static void LoadConfig(string configName)
        {
            try
            {
                if (_configName == configName) return;

                _docFile = $"./Configs/{configName}.xml";
                
                Doc.Load(_docFile);
                _configName = configName;
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
        public static string ConfigStrR(string nodePath, string configName)
        {
            try
            {
                LoadConfig(configName);

                var node = Doc.SelectSingleNode(nodePath);
                if (node?.Attributes == null) return string.Empty;
                
                var val = node.Attributes["value"].Value;
                LogHelper.WriteLogInfo($"Value at xpath {nodePath} ({_docFile}) read: {val}");

                return val;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// General configuration reader, returns with int type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static int ConfigIntR(string nodePath, string configName)
        {
            var str = ConfigStrR(nodePath, configName);
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// General configuration reader, returns with bool type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static bool ConfigBoolR(string nodePath, string configName)
        {
            var str = ConfigStrR(nodePath, configName);
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

        /// <summary>
        /// General configuration reader, returns with double type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static double ConfigDoubleR(string nodePath, string configName)
        {
            var str = ConfigStrR(nodePath, configName);
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// General configuration reader, returns with Datetime type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static DateTime ConfigDatetimeR(string nodePath, string configName)
        {
            var str = ConfigStrR(nodePath, configName);
            try
            {
                return Convert.ToDateTime(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on reading xpath {nodePath}. Exception message: {ex.Message}");
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// General configuration writer, accepts with string type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void ConfigStrW(string nodePath, string configName, string value)
        {
            try
            {
                LoadConfig(configName);

                var node = Doc.SelectSingleNode(nodePath) as XmlElement;
                if (node != null)
                {
                    node.SetAttribute("value", value);
                    Doc.Save(_docFile);
                }
                
                LogHelper.WriteLogInfo($"Value {value} written to xpath {nodePath} ({_docFile}).");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on writing xpath {nodePath}. Exception message: {ex.Message}");
            }
        }

        /// <summary>
        /// General configuration writer, accepts with int type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void ConfigIntW(string nodePath, string configName, int value)
        {
            try
            {
                var str = value.ToString();
                ConfigStrW(nodePath, configName, str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on writing value {value} to xpath {nodePath}. Exception message: {ex.Message}");
            }
        }

        /// <summary>
        /// General configuration writer, accepts with bool type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void ConfigBoolW(string nodePath, string configName, bool value)
        {
            try
            {
                var str = value.ToString();
                ConfigStrW(nodePath, configName, str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on writing value {value} to xpath {nodePath}. Exception message: {ex.Message}");
            }
        }

        /// <summary>
        /// General configuration writer, accepts with double type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void ConfigDoubleW(string nodePath, string configName, double value)
        {
            try
            {
                var str = value.ToString(CultureInfo.InvariantCulture);
                ConfigStrW(nodePath, configName, str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on writing value {value} to xpath {nodePath}. Exception message: {ex.Message}");
            }
        }

        /// <summary>
        /// General configuration writer, accepts with Datetime type.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void ConfigDatetimeW(string nodePath, string configName, DateTime value)
        {
            try
            {
                var str = value.ToString(SysConf.DateTimeFormat);
                ConfigStrW(nodePath, configName, str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError($"Error occured on writing value {value} to xpath {nodePath}. Exception message: {ex.Message}");
            }
        }
    }
}
