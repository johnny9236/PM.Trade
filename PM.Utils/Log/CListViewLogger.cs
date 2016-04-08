using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PM.Utils
{
    public class LogInfo
    { 
        public DateTime OccurTime = DateTime.Now;
        public LogSeverity Level;
        public string Source;
        public string Message;
    }
    public class CListViewLogger
    {
        protected string m_LogName;
        LogSeverity m_OutputLevel = LogSeverity.info;

        int iMaxQueueLen = 100;
        protected Queue<LogInfo> m_MsgQueue;
        ListView m_lvLog = null;

        public CListViewLogger(string LogName)
        {
            m_MsgQueue = new Queue<LogInfo>();
            m_LogName = LogName;
        }

        public CListViewLogger(string LogName,ListView lvLog)
        {
            m_MsgQueue = new Queue<LogInfo>();
            m_LogName = LogName;

            m_lvLog = lvLog;
        }

        ~CListViewLogger()
        {
            m_MsgQueue.Clear();
        }

        public int MaxQueueLen
        {
            get
            {
                return iMaxQueueLen;
            }
            set
            {
                iMaxQueueLen = value;
            }
        }
        public ListView LogView
        {
            get
            {
                return m_lvLog;
            }
            set
            {
                m_lvLog = value;

                InitCtrl();
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

        public void InitCtrl()
        {
            if (m_lvLog != null)
            {
                if (m_lvLog.Columns.Count < 1)
                {
                    m_lvLog.Columns.Clear();
                    m_lvLog.Columns.Add("时间", 135);
                    m_lvLog.Columns.Add("等级", 60);
                    m_lvLog.Columns.Add("来源", 60);
                    m_lvLog.Columns.Add("信息", 650);
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

            LogInfo Info = new LogInfo();
            Info.Level = Level;
            Info.Source = source;
            Info.OccurTime = DateTime.Now;
            Info.Message = message;

            m_MsgQueue.Enqueue(Info);
        }

        public void DisplayUILog()
        {
            while (m_MsgQueue.Count > 0)
            {
                LogInfo Msg = m_MsgQueue.Dequeue();

                DisplayUILog(Msg);
            }
        }

        public void DisplayUILog(LogInfo Msg)
        {
            if (m_lvLog == null) return;

            ListViewItem item = new ListViewItem(Msg.OccurTime.ToString("yy-MM-dd HH:mm:ss:fff"));
            item.SubItems.Add(Msg.Level.ToString());
            item.SubItems.Add(Msg.Source.ToString());
            item.SubItems.Add(Msg.Message);

            item.ForeColor = GetForeColor((int)Msg.Level);

            listViewLogAddItem(item);
        }

        Color[] m_ColorList = new Color[9] { Color.DarkRed, Color.Red, Color.DeepSkyBlue, Color.LightYellow, Color.Yellow,Color.GreenYellow,Color.SlateBlue,Color.Olive,Color.Orange };
        Color GetForeColor(int Index)
        {
            if (Index > -1 && Index < 9)
            {
                return m_ColorList[Index];
            }
            else return Color.Silver;
        }

        delegate void SetLogCallback(ListViewItem item);
        private void listViewLogAddItem(ListViewItem item)
        {
            if (m_lvLog == null) return;

            try
            {
                if (m_lvLog.InvokeRequired)
                {
                    SetLogCallback d = new SetLogCallback(listViewLogAddItem);
                    m_lvLog.Invoke(d, new object[] { item });//引起线程阻塞
                }
                else
                {
                    if (m_lvLog.Items.Count > 1000)
                    {
                        m_lvLog.Items.Clear();
                    }

                    m_lvLog.Items.Add(item);
                    m_lvLog.Refresh();
                    m_lvLog.Items[m_lvLog.Items.Count - 1].EnsureVisible();
                }
            }
            catch (Exception Ex)
            {
                OnError("显示日志异常", Ex);
            }
        }
    }
}
