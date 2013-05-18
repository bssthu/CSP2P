using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CSP2P
{
    // 发送文件时用于将文件分包的类。
    public class FilePacker
    {
        /// <summary>
        /// 读文件流
        /// </summary>
        private FileStream fsReader;

        /// <summary>
        /// 缓存大小
        /// </summary>
        public const int BytesOfPack = 0x1000;

        /// <summary>
        /// 发送文件缓存
        /// </summary>
        private byte[] buf;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FileName">文件名（含路径）</param>
        public FilePacker(string FileName)
        {
            // 打开文件
            try
            {
                fsReader = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：FilePacker");
                Trace.WriteLine(ex.Message);
                return;
            }
            // 初始化缓存数组
            buf = new byte[BytesOfPack];
        }

        /// <summary>
        /// 取得分包数
        /// </summary>
        /// <returns></returns>
        public uint GetPackNumbers()
        {
            ulong size = (ulong)fsReader.Length;
            uint packs = (uint)(size / (ulong)BytesOfPack);
            if (size % BytesOfPack > 0)
            {
                packs++;
            }
            return packs;
        }

        /// <summary>
        /// 取得最后一个包的大小
        /// </summary>
        /// <returns></returns>
        public uint GetSizeOfLastPack()
        {
            ulong size = (ulong)fsReader.Length;
            return (uint)(size % BytesOfPack);
        }

        /// <summary>
        /// 读文件的一个片段
        /// </summary>
        /// <param name="seq">序号</param>
        /// <returns></returns>
        public byte[] GetPack(uint seq)
        {
            try
            {
                fsReader.Position = seq * BytesOfPack;
                fsReader.Read(buf, 0, BytesOfPack);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：GetPack");
                Trace.WriteLine(ex.Message);
            }
            return buf;
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Close()
        {
            try
            {
                fsReader.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：FilePacker.Close");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
