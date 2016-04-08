using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
 
namespace PM.Utils.Quartz
{
    /// <summary>
    /// Quartz job基类(需要被继承)
    /// </summary>
    public abstract class QuartzJobBase : IJob
    {
        ///// <summary>
        ///// JOB状态日志
        ///// </summary>
        //protected internal static readonly ILog jobLog = LogManager.GetLogger("Job.Status");
        ///// <summary>
        ///// 服务错误日志
        ///// </summary>
        //protected internal static readonly ILog serviceErrorLog = LogManager.GetLogger("Service.Error");
        //private static readonly JsonSerializerSettings JsonSettings;
        static QuartzJobBase()
        {
            //JsonSettings = new JsonSerializerSettings();
            //JsonSettings.TypeNameHandling = TypeNameHandling.Auto;
            //JsonSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                // ReadFromJobDataMap(context.MergedJobDataMap);
                InternalExecute(context);
            }
            catch (Exception ex)
            {
                JobExecutionException jex = new JobExecutionException(ex);
                throw jex;
            }
        }
        /// <summary>
        /// 继承类执行
        /// </summary>
        /// <param name="context"></param>
        protected abstract void InternalExecute(IJobExecutionContext context);

        #region JobDataMap 并序列化
        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <returns></returns>
        //public JobDataMap BuildJobDataMap()
        //{
        //    JobDataMap data = new JobDataMap();
        //    foreach (var prop in GetType().GetProperties())
        //    {
        //        object value = prop.GetValue(this, null);
        //        string s = GetPropertyValue(prop);
        //        data.Add(prop.Name, s);
        //    }
        //    return data;
        //}
        //private void ReadFromJobDataMap(JobDataMap data)
        //{
        //    PropertyInfo[] properties = GetType().GetProperties();

        //    foreach (var key in data.Keys)
        //    {
        //        var p = properties.Where(x => x.Name == key).SingleOrDefault();

        //        if (p != null)
        //        {
        //            SetPropertyValue(p, data.GetString(key));
        //        }
        //    }
        //}
        ///// <summary>
        ///// 获取值
        ///// </summary>
        ///// <param name="property"></param>
        ///// <returns></returns>
        //private object GetPropertyValue(PropertyInfo property)
        //{
        //    object value = property.GetValue(this, null);
        //    return JsonConvert.SerializeObject(value, Formatting.None, JsonSettings);
        //}
        ///// <summary>
        ///// 设置值
        ///// </summary>
        ///// <param name="property"></param>
        ///// <param name="value"></param>
        //private void SetPropertyValue(PropertyInfo property, string value)
        //{
        //    object obj = JsonConvert.DeserializeObject(value, property.PropertyType, JsonSettings);
        //    property.SetValue(this, obj, null);
        //}
        #endregion
    }
}
