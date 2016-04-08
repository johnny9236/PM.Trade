using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.TaskBiz.HYSync.QYK.Model;
using PM.Utils;
using PM.Utils.Log;

namespace PM.TaskBiz.HYSync.QYK
{
    /// <summary>
    /// 同步调用
    /// </summary>
    public class QykSyncCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            //获取更新列表
            List<QueueUpdateResult> queueLst = GetQueue();
            if (null != queueLst && queueLst.Count > 0)
            {

                GetCallbackInterface();

                //清除队列
                ClearnQueue(queueLst); 
            }
        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new QykSyncCallBack();
        }

        #region  priv
        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <returns></returns>
        private List<QueueUpdateResult> GetQueue()
        {
            //获取更新列表
            QueueUpdateRequset queue = new QueueUpdateRequset();
            queue.UserToken = ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            var queueResult = queue.GetQueueToUpdate();
            if (queueResult)
            {
                if (null != queue.QueueToUpdateList && queue.QueueToUpdateList.Count > 0)
                {
                    return queue.QueueToUpdateList;
                }
            }
            return null;
        }

        /// <summary>
        /// 清除更新列表
        /// </summary>
        /// <param name="queueLst">更新列表</param>
        private void ClearnQueue(List<QueueUpdateResult> queueLst)
        {
            ClearDealtDataInQueueRequest clearQu = null;
            foreach (var qu in queueLst)
            {
                clearQu = new ClearDealtDataInQueueRequest();
                clearQu.DataID = qu.DataID;
                clearQu.DataType = qu.DataType;
                clearQu.UserToken = ConfigHelper.GetCustomCfg("HY", "QykuserToken");
                if (clearQu.GetClearQueueResult())
                {
                    if (null != clearQu.ClearQueueResult)
                    {
                        if (!clearQu.ClearQueueResult.ExecCuccessfully)
                            LogTxt.WriteEntry(string.Format("清除更新队列失败，类型:{0}[id]为：{1} 信息为:{2}", clearQu.DataType, clearQu.DataID, clearQu.ClearQueueResult.Description), "海盐企业库同步");
                    }
                    else
                    {
                        LogTxt.WriteEntry(string.Format("清除更新队列失败，类型:{0}[id]为：{1}", clearQu.DataType, clearQu.DataID), "海盐企业库同步");
                    }
                }
                else
                {
                    LogTxt.WriteEntry(string.Format("清除更新队列失败，类型:{0}[id]为：{1}", clearQu.DataType, clearQu.DataID), "海盐企业库同步");
                }
            }
        }
        #endregion
    }  
}
