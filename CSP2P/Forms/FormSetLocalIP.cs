using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CSP2P.Properties;

namespace CSP2P
{
    public partial class FormSetLocalIP : Form
    {
        ManagementBaseObject inPar = null;
        ManagementBaseObject outPar = null;
        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection moc = null;

        IPNode[] ipNodes = Settings.Default.IPNodes;

        public FormSetLocalIP()
        {
            InitializeComponent();

            // 初始化WMI
            moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPAddress"] != null)
                {
                    comboBoxNIC.Items.Add(mo["Description"].ToString().Trim());
                }
            }

            // 列出网卡列表
            if (comboBoxNIC.Items.Count > 0)
            {
                comboBoxNIC.SelectedIndex = 0;
                for (int i = 0; i < comboBoxNIC.Items.Count; i++)
                {
                    if (comboBoxNIC.Items[i].ToString() == Settings.Default.defaultNIC)
                    {
                        comboBoxNIC.SelectedIndex = i;
                        break;
                    }
                }
            }

            // 默认配置
            if (null == ipNodes)
            {
                ipNodes = new IPNode[0];
            }
            for (int i = 0; i < ipNodes.Length; i++)
            {
                comboBoxSettings.Items.Add(ipNodes[i].name);
                if (ipNodes[i].name == Settings.Default.defaultIPNodeName)
                {
                    comboBoxSettings.Text = ipNodes[i].name;
                    maskedTextBoxIPAddress.Text = ipNodes[i].maskIPAddress;
                    maskedTextBoxSubnetMask.Text = ipNodes[i].maskSubnet;
                    maskedTextBoxDefaultGateway.Text = ipNodes[i].maskDefaultGateway;
                }
            }
        }

        /// <summary>
        /// 点击“取消”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 点击“应用”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            // 保存
            Settings.Default.IPNodes = ipNodes;
            Settings.Default.defaultIPNodeName = comboBoxSettings.Text;
            Settings.Default.Save();
            // 应用设置
            try
            {
                foreach (ManagementObject mo in moc)
                {
                    if (!((bool)mo["IPEnabled"]) || !(mo["Description"].ToString() == comboBoxNIC.SelectedItem.ToString().Trim()))
                    {
                        continue;
                    }
                    // IP, 子网掩码
                    inPar = mo.GetMethodParameters("EnableStatic");
                    inPar["IPAddress"] = MaskToIPStringArray(maskedTextBoxIPAddress.Text);
                    inPar["SubnetMask"] = MaskToIPStringArray(maskedTextBoxSubnetMask.Text);
                    outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                    // 默认网关
                    inPar = mo.GetMethodParameters("SetGateways");
                    inPar["DefaultIPGateway"] = MaskToIPStringArray(maskedTextBoxDefaultGateway.Text);
                    outPar = mo.InvokeMethod("SetGateways", inPar, null);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：buttonApply_Click");
                Trace.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 将有掩码的字符串转换为合理的字符串
        /// </summary>
        /// <param name="maskString"></param>
        /// <returns></returns>
        private string[] MaskToIPStringArray(string maskString)
        {
            string[] ips = maskString.Split('.');
            string ip = "";
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    ips[i] = Convert.ToInt16(ips[i]).ToString();
                }
                catch //(Exception ex)
                {
                    //Trace.WriteLine("异常位置：MaskToIPStringArray");
                    //Trace.WriteLine(ex.Message);
                }
                ip += ips[i] + ".";
            }
            ip = ip.Substring(0, ip.Length - 1);
            return new string[] { ip };
        }

        /// <summary>
        /// 点击“确定”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonApply_Click(sender, e);
            Close();
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if ((comboBoxSettings.Text == null) || (comboBoxSettings.Text == ""))
            {
                return;
            }
            for (int i = 0; i < ipNodes.Length; i++)
            {
                if (comboBoxSettings.Text == ipNodes[i].name)
                {
                    ipNodes[i].maskIPAddress = maskedTextBoxIPAddress.Text;
                    ipNodes[i].maskSubnet = maskedTextBoxSubnetMask.Text;
                    ipNodes[i].maskDefaultGateway = maskedTextBoxDefaultGateway.Text;
                    return;
                }
            }

            IPNode[] oldNodes = ipNodes;
            ipNodes = new IPNode[oldNodes.Length + 1];
            for (int i = 0; i < oldNodes.Length; i++)
            {
                ipNodes[i] = oldNodes[i];
            }
            ipNodes[oldNodes.Length] = new IPNode();
            ipNodes[oldNodes.Length].name = comboBoxSettings.Text;
            ipNodes[oldNodes.Length].maskIPAddress = maskedTextBoxIPAddress.Text;
            ipNodes[oldNodes.Length].maskSubnet = maskedTextBoxSubnetMask.Text;
            ipNodes[oldNodes.Length].maskDefaultGateway = maskedTextBoxDefaultGateway.Text;

            comboBoxSettings.Items.Add(comboBoxSettings.Text);

            // 保存
            Settings.Default.IPNodes = ipNodes;
            Settings.Default.defaultIPNodeName = comboBoxSettings.Text;
            Settings.Default.Save();
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // 移出设置
            for (int i = 0; i < ipNodes.Length; i++)
            {
                if (ipNodes[i].name == comboBoxSettings.Text)
                {
                    IPNode[] oldNodes = ipNodes;
                    ipNodes = new IPNode[oldNodes.Length - 1];
                    for (int j = 0; j < i; j++)
                    {
                        ipNodes[j] = oldNodes[j];
                    }
                    for (int j = i; j < oldNodes.Length - 1; j++)
                    {
                        ipNodes[j] = oldNodes[j + 1];
                    }
                    break;
                }
            }

            // 移出列表
            for (int i = 0; i < comboBoxSettings.Items.Count; i++)
            {
                if (comboBoxSettings.Items[i].ToString() == comboBoxSettings.Text)
                {
                    comboBoxSettings.Items.RemoveAt(i);
                    break;
                }
            }

            comboBoxSettings.Text = "";
        }

        /// <summary>
        /// 修改设置名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSettings_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBoxSettings.Items.Count; i++)
            {
                if (comboBoxSettings.Items[i].ToString() == comboBoxSettings.Text)
                {
                    maskedTextBoxIPAddress.Text = ipNodes[i].maskIPAddress;
                    maskedTextBoxSubnetMask.Text = ipNodes[i].maskSubnet;
                    maskedTextBoxDefaultGateway.Text = ipNodes[i].maskDefaultGateway;
                    break;
                }
            }
        }
    }
}
