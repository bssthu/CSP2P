using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 存储IP配置

namespace CSP2P
{
    public class IPNode
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string name = "";

        public string maskIPAddress = null;
        public string maskSubnet = null;
        public string maskDefaultGateway = null;
    }
}
