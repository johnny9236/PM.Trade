using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.Utils
{
    /// <summary>
    /// 定义错误信息
    /// </summary>
    internal class ErrorInfo
    {
        public static readonly string ConfigItemNotFound = "配置项: [{0}] 不存在。";
        public static readonly string ConfigItemNotSet = "配置项 [{0}] 没有被设置。";
        public static readonly string StringFormatInvalid = "字符串的格式无效。";
        public static readonly string TypeIsNotEnum = "指定的类型不是枚举类型。";
        public static readonly string TypeNotFound = "类型 [{0}] 没有找到。";
        public static readonly string HexLenIsWrong = "十六进制的字节数组的长度不正确。";
        public static readonly string LoadUCFailed = "加载用户控件 [{0}] 失败.";
        public static readonly string AjaxAssemblyNameIsNull = "MyAjaxMethodV1Handler.AjaxAssemblyName 没有设置。";
        public static readonly string KeyNotFoundInRequest = "当前请求中没有找到名为 [{0}] 的参数项。";
        public static readonly string PropertyNotFound = "属性 [{0}] 没有找到。";
        public static readonly string MethodNotFound = "根据指定的方法名 [{0}] 找不到相应的实现方法。";
        public static readonly string InvalidRequest = "无效的请求，不能被解析为类/方法的调用。";
        public static readonly string NotFoundMethod = "没有找到指定的方法。";
        public static readonly string ViewDataTypeIsWrong = "参数viewData的类型与用户控件的参数类型不一致。";
        public static readonly string UrlParameterNotSet = "URL参数项 [{0}] 没有设置。";
    }
}
