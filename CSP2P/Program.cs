using System;
using System.Windows.Forms;

// 应用程序的入口点，本文件是自动生成的，与实验报告关系不大

namespace CSP2P
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(args));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.HelpLink,
                    "发生了意料之外的错误……");
                System.Environment.Exit(ex.Data.Count);
            }
        }
    }
}
