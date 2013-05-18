using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// 包含协议处理类
// 多线程机制中，每次都需要新建一个ProtocalHandler的副本以免冲突

namespace CSP2P
{
    /// <summary>
    /// 协议处理类
    /// </summary>
    public class ProtocalHandler
    {
        /// <summary>
        /// 接收的协议的xml文本
        /// </summary>
        public string protocalText;

        /// <summary>
        /// 设置用户名，按base64编码
        /// </summary>
        public static string UserName
        {
            set
            {
                _UserNameBase64 = Convert.ToBase64String(
                    Encoding.Unicode.GetBytes(value));
            }
        }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProtocalHandler()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName">自己的用户名</param>
        public ProtocalHandler(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// 用户名，
        /// base64编码
        /// </summary>
        private static string _UserNameBase64;

        /// <summary>
        /// 按照协议封装数据报文，形如
        /// <CSP2P><type>segType</type>
        /// <data>segText</data>
        /// </summary>
        /// <param name="segType">报文类型</param>
        /// <param name="segText">数据</param>
        /// <returns></returns>
        public string Pack(string segType, string segText = null)
        {
            if (segType == null)
            {
                throw(new Exception("必须指定报文的type"));
            }
            string pack = String.Format("<CSP2P><type>{0}</type>", segType);
            pack = String.Format("{0}<name>{1}</name>", pack, _UserNameBase64);
            if (segText != null)
            {
                pack = String.Format("{0}<data>{1}</data>", pack, segText);
            }
            pack = String.Format("{0}</CSP2P>", pack);
            return pack;
        }

        /// <summary>
        /// 向协议文本追加内容
        /// </summary>
        /// <param name="protocalText">原协议文本</param>
        /// <param name="typeName">追加的类型</param>
        /// <param name="data">追加的内容</param>
        /// <returns>新协议文本</returns>
        public string Append(string protocalText, string typeName, string data)
        {
            if (!protocalText.EndsWith("</CSP2P>"))
            {
                return null;
            }
            protocalText = String.Format("{0}<{1}>{2}</{1}></CSP2P>",
                protocalText.Substring(0,
                    protocalText.Length - "</CSP2P>".Length),
                typeName, data);
            return protocalText;
        }

        /// <summary>
        /// 将base64编码的字符串解码
        /// </summary>
        /// <param name="base64">按base64编码的字符串</param>
        /// <returns></returns>
        public string Base64stringToString(string base64)
        {
            try
            {
                return Encoding.Unicode.GetString(Convert.FromBase64String(base64));
            }
            catch
            {
                Trace.WriteLine(base64 + " 解码失败");
                return null;
            }
        }

        /// <summary>
        /// 将字符串用base64编码
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <returns></returns>
        public string StringToBase64string(string str)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(str));
        }

        /// <summary>
        /// 提取完整的协议文本并记录
        /// 形如"<CSP2P>abc</CSP2P>"
        /// </summary>
        /// <param name="input"></param>
        /// <returns>是否成功提取到协议</returns>
        public bool SetXmlText(string input)
        {
            protocalText = input;
            protocalText = GetElementTextByTag("CSP2P", 0);
            if (protocalText == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 按照标签访问内容，返回一条
        /// </summary>
        /// <param name="elementName">元素名</param>
        /// <param name="input">要处理的字符串</param>
        /// <param name="id">Groups的下标，0为全部，1为内容</param>
        /// <returns></returns>
        public string GetElementTextByTag(string tag, int id = 1)
        {
            string pattern = String.Format(@"<{0}>(.*?)</{0}>", tag);
            Regex regex;
            try
            {
                regex = new Regex(pattern);
                if (regex.IsMatch(protocalText))
                {
                    return regex.Match(protocalText).Groups[id].Value;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("错误位置：GetElementTextByTag");
                Trace.WriteLine(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 按照标签访问内容，返回数组
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string[] GetElementsTextByTag(string tag, int id = 1)
        {
            string pattern = String.Format(@"<{0}>(.*?)</{0}>", tag);
            Regex regex;
            try
            {
                regex = new Regex(pattern);
                if (regex.IsMatch(protocalText))
                {
                    MatchCollection matchCollection = regex.Matches(protocalText);
                    string[] texts = new string[matchCollection.Count];
                    for (int i = 0; i < matchCollection.Count; i++)
                    {
                        texts[i] = matchCollection[i].Groups[id].Value;
                    }
                    return texts;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("错误位置：GetElementsTextByTag");
                Trace.WriteLine(ex.Message);
                return null;
            }
            return null;
        }
    }
}
