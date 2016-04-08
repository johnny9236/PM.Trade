using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;

namespace PM.Utils.Log
{
    public class LogTxt
    {
        /// <summary>
        /// 写入文本日志
        /// </summary>
        /// <param name="Description">日志</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void WriteEntry(string Description, string filestyle)
        {
            //EventLogTxt.WriteEntry("页面alipayto.aspx中生成订单编号时出错： " + ex.Message, "DebugLog");
            string filePath;
            StreamWriter fs = null;
            try
            {
                filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "CustomLog\\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath = filePath + filestyle + DateTime.Today.ToString("yyyy-MM-dd") + "_EventLog.txt";

                if (!File.Exists(filePath))
                    fs = File.CreateText(filePath);
                else
                    fs = File.AppendText(filePath);
                fs.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ---  ") + Description);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
