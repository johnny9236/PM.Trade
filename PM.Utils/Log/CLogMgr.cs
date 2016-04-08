using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PM.Utils
{
    public class CLogMgr
    {
        CListViewLogger m_AppUILog;
        CFileLogger m_AppFileLog;

        CListViewLogger m_ErrorUILog;
        CFileLogger m_ErrorFileLog;

        static CLogMgr g_LogMgr = new CLogMgr();

        public CLogMgr()
        {
            m_AppUILog = new CListViewLogger("SvrApp");
            m_AppFileLog = new CFileLogger("SvrApp");

            m_ErrorUILog = new CListViewLogger("SvrError");
            m_ErrorFileLog = new CFileLogger("SvrError");

            m_AppUILog.OnErrorEvent += new CListViewLogger.OnErrorEventHandler(OnError);
            m_ErrorUILog.OnErrorEvent += new CListViewLogger.OnErrorEventHandler(OnError);

            m_AppFileLog.OnErrorEvent += new CFileLogger.OnErrorEventHandler(OnError);
            m_ErrorFileLog.OnErrorEvent +=new CFileLogger.OnErrorEventHandler( OnError);
        }
        ~CLogMgr()
        {
#if DEBUG
            Console.WriteLine("~CLogMgr");
#endif
        }
        public static CLogMgr G_Instance
        {
            get
            {
                return g_LogMgr;
            }
        }
        public void OnError(string ExtraMsg, Exception Ex)
        {
#if DEBUG
            MessageBox.Show(Ex.Message, ExtraMsg);
#endif
        }
        public LogSeverity AppOutputLevel
        {
            set
            {
                m_AppUILog.OutputLevel = value;
                m_AppFileLog.OutputLevel = value;
            }
            get
            {
                return m_AppUILog.OutputLevel;
            }
        }

        public LogSeverity ErrorOutputLevel
        {
            set
            {
                m_ErrorUILog.OutputLevel = value;
                m_ErrorFileLog.OutputLevel = value;
            }
            get
            {
                return m_ErrorUILog.OutputLevel;
            }
        }

        public ListView AppLogView
        {
            get
            {
                return m_AppUILog.LogView;
            }
            set
            {
                m_AppUILog.LogView = value;

            }
        }

        public ListView ErrorLogView
        {
            get
            {
                return m_ErrorUILog.LogView;
            }
            set
            {
                m_ErrorUILog.LogView = value;

            }
        }

        public void WriteAppLog(LogSeverity Level, string source, string message)
        {
            m_AppUILog.WriteLog(Level, source, message);
            m_AppFileLog.WriteLog(Level, source, message);
        }

        public void WriteErrorLog(LogSeverity Level, string source, Exception Ex)
        {
            WriteErrorLog(Level, source, Ex.Message + ":" + Ex.StackTrace);
        }
        public void WriteErrorLog(LogSeverity Level, string source, string message)
        {
            m_ErrorUILog.WriteLog(Level, source, message);
            m_ErrorFileLog.WriteLog(Level, source, message);
        }

        public void DisplayUILog()
        {
            m_AppUILog.DisplayUILog();
            m_ErrorUILog.DisplayUILog();
        }
    }
}
