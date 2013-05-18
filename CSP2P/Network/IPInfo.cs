using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CSP2P
{
    /// <summary>
    /// 对方的地址信息，
    /// 包括IP地址、TCP监听端口、UDP监听端口
    /// </summary>
    public class IPInfo
    {
        /// <summary>
        /// IP地址及TCP监听端口（设置）
        /// </summary>
        private IPAddress _IpAddress;

        private int _TCPListeningPort = 0;

        private int _TCPSocketPort = 0;

        /// <summary>
        /// UDP监听端口（设置）
        /// </summary>
        private int _UDPPort;

        /// <summary>
        /// IP地址及TCP监听端口（访问）
        /// </summary>
        public IPEndPoint IpEndPoint
        {
            get
            {
                if (_TCPSocketPort == 0)
                {
                    return new IPEndPoint(_IpAddress, _TCPListeningPort);
                }
                else
                {
                    return new IPEndPoint(_IpAddress, _TCPSocketPort);
                }
            }
        }

        /// <summary>
        /// IP地址（访问）
        /// </summary>
        public IPAddress IpAddress
        {
            get
            {
                return _IpAddress;
            }
        }

        /// <summary>
        /// TCP监听端口（访问）
        /// </summary>
        public int TCPListeningPort
        {
            get
            {
                return _TCPListeningPort;
            }
        }

        /// <summary>
        /// TCP通信中的端口（访问）
        /// </summary>
        public int TCPSocketPort
        {
            get
            {
                return _TCPSocketPort;
            }
        }

        /// <summary>
        /// UDP监听端口（访问）
        /// </summary>
        public int UDPPort
        {
            get
            {
                return _UDPPort;
            }
        }

        /// <summary>
        /// 构造函数，UDP广播
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="tcpPort">TCP监听端口</param>
        /// <param name="udpPort">UDP监听端口</param>
        public IPInfo(IPAddress ipAddress, int tcpListeningPort, int udpPort)
        {
            _IpAddress = ipAddress;
            _TCPListeningPort = tcpListeningPort;
            _UDPPort = udpPort;
        }

        /// <summary>
        /// 构造函数，通信开始
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public IPInfo(IPEndPoint ipEndPoint)
        {
            _IpAddress = ipEndPoint.Address;
            _TCPSocketPort = ipEndPoint.Port;
        }

        /// <summary>
        /// 更新信息，UDP广播
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="tcpListeningPort">TCP监听端口</param>
        /// <param name="udpPort">UDP监听端口</param>
        public void Update(IPAddress ipAddress, int tcpListeningPort, int udpPort)
        {
            _IpAddress = ipAddress;
            _TCPListeningPort = tcpListeningPort;
            _UDPPort = udpPort;
        }

        /// <summary>
        /// 更新信息，通信开始
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void Update(IPEndPoint ipEndPoint)
        {
            _IpAddress = ipEndPoint.Address;
            _TCPSocketPort = ipEndPoint.Port;
        }

        /// <summary>
        /// 通信结束
        /// </summary>
        public void OnCloseSocket()
        {
            _TCPSocketPort = 0;
        }
    }
}
