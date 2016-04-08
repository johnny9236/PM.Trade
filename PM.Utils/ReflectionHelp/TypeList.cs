using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace PM.Utils.ReflectionHelp
{
    /// <summary>
    /// 类型定义
    /// </summary>
    internal static class TypeList
    {
        public static readonly Type _bool = typeof(bool);
        public static readonly Type _byte = typeof(byte);
        public static readonly Type _char = typeof(char);
        public static readonly Type _DateTime = typeof(DateTime);
        public static readonly Type _decimal = typeof(decimal);
        public static readonly Type _double = typeof(double);
        public static readonly Type _float = typeof(float);
        public static readonly Type _Guid = typeof(Guid);
        public static readonly Type _HttpContext = typeof(HttpContext);
        public static readonly Type _HttpRequest = typeof(HttpRequest);
        public static readonly Type _IEnumerable = typeof(IEnumerable);
        public static readonly Type _int = typeof(int);
        public static readonly Type _long = typeof(long);
        public static readonly Type _nullable = typeof(Nullable<>);
        public static readonly Type _object = typeof(object);
        public static readonly Type _sbyte = typeof(sbyte);
        public static readonly Type _short = typeof(short);
        public static readonly Type _string = typeof(string);
        public static readonly Type _uint = typeof(uint);
        public static readonly Type _ulong = typeof(ulong);
        public static readonly Type _ushort = typeof(ushort);
        public static readonly Type _void = typeof(void);
    }
}
