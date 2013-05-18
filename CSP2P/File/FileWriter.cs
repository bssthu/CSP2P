using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CSP2P
{
    public class FileWriter
    {
        /// <summary>
        /// 写文件流
        /// </summary>
        private FileStream fsWriter;

        /// <summary>
        /// 缓存大小
        /// </summary>
        private const int BytesOfPack = 0x1000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FileName">文件名（含路径）</param>
        public FileWriter(string FileName)
        {
            // 打开文件
            try
            {
                fsWriter = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：FileWriter");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 写入一段数据
        /// </summary>
        /// <param name="seq">该段的序号</param>
        /// <param name="buf">要写入的数据</param>
        /// <param name="count">要写入的字节数</param>
        public void WriteBytes(uint seq, byte[] buf, uint count)
        {
            try
            {
                fsWriter.Position = seq * BytesOfPack;
                fsWriter.Write(buf, 0, (int)count);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：WriteBytes");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Close()
        {
            try
            {
                fsWriter.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：FileWriter.Close");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
