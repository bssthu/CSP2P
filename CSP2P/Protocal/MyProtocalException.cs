using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP2P
{
    // 使用自定协议时发生的异常
    public class MyProtocalException : Exception
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MyProtocalException()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public MyProtocalException(string message)
            : base(message)
        {
        }
    }
}
