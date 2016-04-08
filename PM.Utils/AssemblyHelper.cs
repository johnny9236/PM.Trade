using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace PM.Utils
{
    /// <summary>
    /// <para>　</para>
    /// 类名：常用工具类——应用程序属性信息访问类  
    public class AssemblyHelper
    {
        /// <summary>
        /// 获取应用程序集的标题
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyTitle()
        {
            string assemblyTitle = string.Empty;
            object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (customAttributes.Length > 0)
            {
                AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
                if (string.IsNullOrEmpty(assemblyTitleAttribute.Title))
                    assemblyTitle = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                else
                    assemblyTitle = assemblyTitleAttribute.Title;
            }
            return assemblyTitle;
        }
        /// <summary>
        /// 获取应用程序产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProduct()
        {
            object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (customAttributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)customAttributes[0]).Product;
        }
        /// <summary>
        /// 获取应用程序版本
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// 获取应用程序说明
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyDescription()
        {
            object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (!false && customAttributes.Length != 0)
            {
                return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
            }
            return "";
        }
        /// <summary>
        /// 获取应用程序版权信息
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {
            object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (customAttributes.Length != 0)
            {
                return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
            }
            return "";
        }
        /// <summary>
        /// 获取应用程序公司名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCompany()
        {
            object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (customAttributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
        }
        /// <summary>
        /// 获取应用程序显示名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyAppFullName()
        {
            return Assembly.GetExecutingAssembly().FullName.ToString();
        }
    }
}
