using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PM.Utils
{
    public enum LogSeverity : int
    { 
        fatal = 0,//Emergency: system is unusable,
        emergency = 1,	// Emergency: system is unusable
        alert = 2,	// Alert: action must be taken immediately
        critical = 3,	// Critical: critical conditions
        error = 4,	// Error: error conditions
        warning = 5,	// Warning: warning conditions
        notice = 6,	// Notice: normal but significant condition
        info = 7,	// Informational: informational messages
        debug = 8,	// Debug: debug-level messages
    }
    public class CFileLogger
    {
        protected int LogMaxContent = 4096000; // 文件大小4M
        protected string m_strFileName = "";

        protected string m_LogName;
        // 文件句柄
        //private static Stream _fileStream = null;
        private Stream _fileStream = null;

        LogSeverity m_OutputLevel = LogSeverity.info;

        Mutex mutex = null;//Add by Legahero 20070829
        public CFileLogger(string LogName)
        {
            mutex = new Mutex();

            CreateFileLog(LogName);
            m_LogName = LogName;
        }

        ~CFileLogger()
        {
            if (null != _fileStream)
            {
                _fileStream.Close();
            }
        }

        public string LogName
        {
            get
            {
                return m_LogName;
            }
        }
        public LogSeverity OutputLevel
        {
            set
            {
                m_OutputLevel = value;
            }
            get
            {
                return m_OutputLevel;
            }
        }

        void CreateFileLog(string LogName)
        {
            string LogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            if (null == _fileStream)
            {
                try
                {
                    string strFileName = LogPath + LogName+".log";
                    _fileStream = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    m_strFileName = strFileName;
                }
                catch(Exception ex)
                {
                    OnError("日志输出", ex);
                }
            }
        }
        public delegate void OnErrorEventHandler(string ExtraMsg, Exception Ex);

        public OnErrorEventHandler OnErrorEvent;
        public void OnError(string ExtraMsg, Exception Ex)
        {
            if (OnErrorEvent != null)
            {
                OnErrorEvent(ExtraMsg, Ex);
            }
        }
        public void WriteLog(LogSeverity Level, string source, string message)
        {
            if (Level > m_OutputLevel) return;

            lock (this)
            {
                mutex.WaitOne();//Add by legahero 20070829

                _WriteLog(Level, source, message);

                mutex.ReleaseMutex();
            }
        }
        void _WriteLog(LogSeverity Level, string source, string message)
        {
            if (_fileStream == null) return;

            try
            {
                _fileStream.Seek(0, SeekOrigin.End);
                StreamWriter sw = new StreamWriter(_fileStream, Encoding.Unicode);
                //StreamWriter sw = new StreamWriter(_fileStream,);
                sw.WriteLine("%%"+DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff") + " " + Level.ToString() + " " + source + ":" + message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                OnError("日志输出",ex);
                //throw ex;//不能抛出异常
            }

            try
            {
                //自动备份  Add by legahero 200708
                if (_fileStream.Length > LogMaxContent)
                {
                    _fileStream.Close();

                    FileInfo Finfo = new FileInfo(m_strFileName);

                    string LogfileName = Finfo.Name;
                    string PathNameMove = m_strFileName.Substring(0, m_strFileName.LastIndexOf("\\")) + "\\" + DateTime.Now.ToString("yyyyMMddhhmm") + LogfileName;


                    Finfo.MoveTo(PathNameMove);
                    //Finfo.CopyTo(PathNameMove);
                    //Finfo.Delete();

                    //删除重新建立
                    _fileStream = new FileStream(m_strFileName, FileMode.OpenOrCreate, FileAccess.Write);
                }
            }
            catch (Exception ex)
            {
                OnError("日志输出",ex);
                //throw ex;//不能抛出异常
            }
        }
    }
}
