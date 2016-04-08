using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.CommProtocalBackInterface
{
    /// <summary>
    /// 查询通用操作后续业务接口
    /// </summary>
    public interface ICommProtocalBackBiz
    {
        /// <summary>
        /// 动作
        /// </summary>
        /// <param name="obj">操作参数对象</param>
        void Action(dynamic obj);
    }
}
