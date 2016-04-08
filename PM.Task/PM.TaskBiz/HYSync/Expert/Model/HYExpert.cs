using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.TaskBiz.HYSync.Expert.Model
{
    /// <summary>
    /// 获取专家列表
    /// </summary>
    public class HYExpert
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 抽取时间
        /// </summary>
        public string CQDate { get; set; }
    }
}
