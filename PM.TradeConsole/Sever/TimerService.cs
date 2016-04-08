using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.TradeConsole.Sever
{
    /// <summary>
    /// 定时任务服务
    /// </summary>
    public class TimerService
    {
        static Quartz.IScheduler sched = null;
        /// <summary>
        /// 定时服务启动
        /// </summary>
        public static void TimerServiceStart()
        {
            //定时任务开始
            Quartz.ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory();
            sched = sf.GetScheduler();
            sched.Start();//启动 
            Console.WriteLine("-------------------启动Job-------------------");
        }
        /// <summary>
        /// 停止job
        /// </summary>
        public static void TimerServiceStop()
        {
            if (null != sched)
            {
                try
                {
                    sched.Shutdown();
                    Console.WriteLine("-------------------停止Job-------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
