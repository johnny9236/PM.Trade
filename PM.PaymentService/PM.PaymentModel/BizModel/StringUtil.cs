using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentModel.BizModel
{
    /// <summary>
    /// 获取字符串长度    （tool中有 避免调用tool的dll ）
    /// </summary>
    public class StringUtil
    {
        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static int Text_Length(string Text)
        {
            int len = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return len;
        }
    }
}
