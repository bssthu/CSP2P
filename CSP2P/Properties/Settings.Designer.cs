﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSP2P.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// 记住学号
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("记住学号")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool remUser {
            get {
                return ((bool)(this["remUser"]));
            }
            set {
                this["remUser"] = value;
            }
        }
        
        /// <summary>
        /// 记住的学号
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("记住的学号")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string User {
            get {
                return ((string)(this["User"]));
            }
            set {
                this["User"] = value;
            }
        }
        
        /// <summary>
        /// 主界面默认大小
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("主界面默认大小")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("285, 569")]
        public global::System.Drawing.Size windowSizeMain {
            get {
                return ((global::System.Drawing.Size)(this["windowSizeMain"]));
            }
            set {
                this["windowSizeMain"] = value;
            }
        }
        
        /// <summary>
        /// 主界面距左上角距离
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("主界面距左上角距离")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50, 50")]
        public global::System.Drawing.Point windowPosMain {
            get {
                return ((global::System.Drawing.Point)(this["windowPosMain"]));
            }
            set {
                this["windowPosMain"] = value;
            }
        }
        
        /// <summary>
        /// 是否记住窗口位置信息
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("是否记住窗口位置信息")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool remWindowMain {
            get {
                return ((bool)(this["remWindowMain"]));
            }
            set {
                this["remWindowMain"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Windows.Forms.ListViewItem friends {
            get {
                return ((global::System.Windows.Forms.ListViewItem)(this["friends"]));
            }
            set {
                this["friends"] = value;
            }
        }
        
        /// <summary>
        /// 默认网卡名
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("默认网卡名")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string defaultNIC {
            get {
                return ((string)(this["defaultNIC"]));
            }
            set {
                this["defaultNIC"] = value;
            }
        }
        
        /// <summary>
        /// IP配置
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("IP配置")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public IPNode[] IPNodes {
            get {
                return ((IPNode[])(this["IPNodes"]));
            }
            set {
                this["IPNodes"] = value;
            }
        }
        
        /// <summary>
        /// 默认IP配置名
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("默认IP配置名")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string defaultIPNodeName {
            get {
                return ((string)(this["defaultIPNodeName"]));
            }
            set {
                this["defaultIPNodeName"] = value;
            }
        }
    }
}