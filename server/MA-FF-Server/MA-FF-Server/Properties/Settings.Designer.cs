﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAnalyzer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\thomas\\Documents\\WebAnalyzer\\")]
        public string Datalocation {
            get {
                return ((string)(this["Datalocation"]));
            }
            set {
                this["Datalocation"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebAnalyzer")]
        public string Applicationname {
            get {
                return ((string)(this["Applicationname"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FirstStart {
            get {
                return ((bool)(this["FirstStart"]));
            }
            set {
                this["FirstStart"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("experiment.xml")]
        public string ExperimentFilename {
            get {
                return ((string)(this["ExperimentFilename"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("settings.xml")]
        public string SettingsFilename {
            get {
                return ((string)(this["SettingsFilename"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("participants.xml")]
        public string ParticipantsFilename {
            get {
                return ((string)(this["ParticipantsFilename"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("data\\{1}\\raw")]
        public string RawdataLocation {
            get {
                return ((string)(this["RawdataLocation"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("data\\{1}\\statistics\\")]
        public string StatisticsLocation {
            get {
                return ((string)(this["StatisticsLocation"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UseMouseTracking {
            get {
                return ((bool)(this["UseMouseTracking"]));
            }
            set {
                this["UseMouseTracking"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8888")]
        public int WebsocketPort {
            get {
                return ((int)(this["WebsocketPort"]));
            }
            set {
                this["WebsocketPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ETConnectLocal {
            get {
                return ((bool)(this["ETConnectLocal"]));
            }
            set {
                this["ETConnectLocal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string ETReceiveIP {
            get {
                return ((string)(this["ETReceiveIP"]));
            }
            set {
                this["ETReceiveIP"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5555")]
        public int ETReceivePort {
            get {
                return ((int)(this["ETReceivePort"]));
            }
            set {
                this["ETReceivePort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string ETSentIP {
            get {
                return ((string)(this["ETSentIP"]));
            }
            set {
                this["ETSentIP"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4444")]
        public int ETSentPort {
            get {
                return ((int)(this["ETSentPort"]));
            }
            set {
                this["ETSentPort"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("log\\")]
        public string LogsLocation {
            get {
                return ((string)(this["LogsLocation"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int LogCount {
            get {
                return ((int)(this["LogCount"]));
            }
            set {
                this["LogCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int WSMessageDelay {
            get {
                return ((int)(this["WSMessageDelay"]));
            }
            set {
                this["WSMessageDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int MouseTrackingInterval {
            get {
                return ((int)(this["MouseTrackingInterval"]));
            }
            set {
                this["MouseTrackingInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int TestrunSaveCheckIntervall {
            get {
                return ((int)(this["TestrunSaveCheckIntervall"]));
            }
            set {
                this["TestrunSaveCheckIntervall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int TestrunDataWaitDuration {
            get {
                return ((int)(this["TestrunDataWaitDuration"]));
            }
            set {
                this["TestrunDataWaitDuration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public int DataTimeout {
            get {
                return ((int)(this["DataTimeout"]));
            }
            set {
                this["DataTimeout"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("data\\{1}\\data")]
        public string ExperimentDataLocation {
            get {
                return ((string)(this["ExperimentDataLocation"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int WebsocketNegotiationTimeout {
            get {
                return ((int)(this["WebsocketNegotiationTimeout"]));
            }
            set {
                this["WebsocketNegotiationTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int WebsocketSentTimeout {
            get {
                return ((int)(this["WebsocketSentTimeout"]));
            }
            set {
                this["WebsocketSentTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int WebsocketReceiveTimeout {
            get {
                return ((int)(this["WebsocketReceiveTimeout"]));
            }
            set {
                this["WebsocketReceiveTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int WebsocketPingTimeout {
            get {
                return ((int)(this["WebsocketPingTimeout"]));
            }
            set {
                this["WebsocketPingTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool WebsocketUseNagle {
            get {
                return ((bool)(this["WebsocketUseNagle"]));
            }
            set {
                this["WebsocketUseNagle"] = value;
            }
        }
    }
}
