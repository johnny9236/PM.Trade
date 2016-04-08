using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace PM.Utils.ReflectionHelp
{
    internal static class ReflectionHelper
    {
        private static Hashtable s_methodTable = Hashtable.Synchronized(new Hashtable(0x1000));
        private static Hashtable s_propTable = Hashtable.Synchronized(new Hashtable(0x400));

        public static PropertyInfo GetInstancePropertyInfo(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            string str = name + "@" + type.FullName;
            PropertyInfo property = (PropertyInfo)s_propTable[str];
            if (property == null)
            {
                property = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    throw new InvalidOperationException(string.Format(ErrorInfo.PropertyNotFound, name));
                }
                s_propTable[str] = property;
            }
            return property;
        }

        public static MethodInfoEx GetMethodInfo(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            string str = name + "@" + type.FullName;
            MethodInfoEx ex = (MethodInfoEx)s_methodTable[str];
            if (ex == null)
            {
                MethodInfo method = type.GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (method == null)
                {
                    throw new InvalidOperationException(string.Format(ErrorInfo.MethodNotFound, name));
                }
                ParameterInfo[] parameters = method.GetParameters();
                ex = new MethodInfoEx
                {
                    MethodInfo = method,
                    ParameterInfos = parameters
                };
                s_methodTable[str] = ex;
            }
            return ex;
        }
    }
}