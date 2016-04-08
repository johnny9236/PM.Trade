using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.Utils.ExecptionHelp
{
    /// <summary>
    /// 处理异常消息文本的工具类
    /// </summary>
    public static class ExecptionHelper
    {
        private static readonly string s_messageSeparator = "\r\n\r\n -> ";

        ///   <summary>
        ///   返回异常的可供显示信息
        ///  </summary>
        ///  <param name="ex">异常对象</param>
        ///  <returns>返回异常的可供显示信息</returns>
        public static string GetExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(ex.Message);
            for (Exception exception = ex.InnerException; exception != null; exception = exception.InnerException)
            {
                builder.Append(s_messageSeparator);
                builder.Append(string.Format("{0} ({1})", exception.Message, exception.GetType().Name));
            }
            return builder.ToString();
        }

        ///   <summary>
        ///   返回异常的全部信息，包括：Message, Type, InnerException, Source, Method, Stack Trace
        ///    </summary>
        ///   <param name="ex">异常对象</param>
        ///  <returns>返回异常的全部信息，包括：Message, Type, InnerException, Source, Method, Stack Trace</returns>
        public static string GetExecptionDetailInfo(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("{0} ({1})", ex.Message, ex.GetType().Name));
            for (Exception exception = ex.InnerException; exception != null; exception = exception.InnerException)
            {
                builder.Append(s_messageSeparator);
                builder.Append(string.Format("{0} ({1})", exception.Message, exception.GetType().Name));
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.AppendFormat("Exception generated at: {0}\r\n", DateTime.Now.ToString("u"));
            builder2.AppendLine("Message: " + builder.ToString());
            builder2.AppendLine("Source: " + ex.Source);
            builder2.AppendLine("Method: " + ex.TargetSite);
            builder2.AppendLine("Stack Trace: ");
            builder2.AppendLine(ex.StackTrace);
            return builder2.ToString();
        }
    }
}
