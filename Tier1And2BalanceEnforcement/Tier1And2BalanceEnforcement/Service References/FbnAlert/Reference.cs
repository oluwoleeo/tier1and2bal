﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tier1And2BalanceEnforcement.FbnAlert {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SMSMessage", Namespace="http://schemas.datacontract.org/2004/07/SMSAppws")]
    [System.SerializableAttribute()]
    public partial class SMSMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AppCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MobileNoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountNumber {
            get {
                return this.AccountNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNumberField, value) != true)) {
                    this.AccountNumberField = value;
                    this.RaisePropertyChanged("AccountNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppCode {
            get {
                return this.AppCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.AppCodeField, value) != true)) {
                    this.AppCodeField = value;
                    this.RaisePropertyChanged("AppCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MobileNo {
            get {
                return this.MobileNoField;
            }
            set {
                if ((object.ReferenceEquals(this.MobileNoField, value) != true)) {
                    this.MobileNoField = value;
                    this.RaisePropertyChanged("MobileNo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Output", Namespace="http://schemas.datacontract.org/2004/07/SMSAppws")]
    [System.SerializableAttribute()]
    public partial class Output : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReasonField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool StatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Reason {
            get {
                return this.ReasonField;
            }
            set {
                if ((object.ReferenceEquals(this.ReasonField, value) != true)) {
                    this.ReasonField = value;
                    this.RaisePropertyChanged("Reason");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SMSMessage2", Namespace="http://schemas.datacontract.org/2004/07/SMSAppws")]
    [System.SerializableAttribute()]
    public partial class SMSMessage2 : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AppCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountNumber {
            get {
                return this.AccountNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNumberField, value) != true)) {
                    this.AccountNumberField = value;
                    this.RaisePropertyChanged("AccountNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppCode {
            get {
                return this.AppCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.AppCodeField, value) != true)) {
                    this.AppCodeField = value;
                    this.RaisePropertyChanged("AppCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GHSMSProfile", Namespace="http://schemas.datacontract.org/2004/07/SMSAppws")]
    [System.SerializableAttribute()]
    public partial class GHSMSProfile : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CurrencyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SolIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountName {
            get {
                return this.AccountNameField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNameField, value) != true)) {
                    this.AccountNameField = value;
                    this.RaisePropertyChanged("AccountName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Currency {
            get {
                return this.CurrencyField;
            }
            set {
                if ((object.ReferenceEquals(this.CurrencyField, value) != true)) {
                    this.CurrencyField = value;
                    this.RaisePropertyChanged("Currency");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SolId {
            get {
                return this.SolIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SolIdField, value) != true)) {
                    this.SolIdField = value;
                    this.RaisePropertyChanged("SolId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FbnAlert.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendMessage", ReplyAction="http://tempuri.org/IService/SendMessageResponse")]
        Tier1And2BalanceEnforcement.FbnAlert.Output SendMessage(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendMessage", ReplyAction="http://tempuri.org/IService/SendMessageResponse")]
        System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendMessageAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendSMSAcc", ReplyAction="http://tempuri.org/IService/SendSMSAccResponse")]
        Tier1And2BalanceEnforcement.FbnAlert.Output SendSMSAcc(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage2 message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendSMSAcc", ReplyAction="http://tempuri.org/IService/SendSMSAccResponse")]
        System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendSMSAccAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage2 message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendMessageGH", ReplyAction="http://tempuri.org/IService/SendMessageGHResponse")]
        Tier1And2BalanceEnforcement.FbnAlert.Output SendMessageGH(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message, Tier1And2BalanceEnforcement.FbnAlert.GHSMSProfile profile, string tranid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SendMessageGH", ReplyAction="http://tempuri.org/IService/SendMessageGHResponse")]
        System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendMessageGHAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message, Tier1And2BalanceEnforcement.FbnAlert.GHSMSProfile profile, string tranid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : Tier1And2BalanceEnforcement.FbnAlert.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<Tier1And2BalanceEnforcement.FbnAlert.IService>, Tier1And2BalanceEnforcement.FbnAlert.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Tier1And2BalanceEnforcement.FbnAlert.Output SendMessage(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message) {
            return base.Channel.SendMessage(message);
        }
        
        public System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendMessageAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message) {
            return base.Channel.SendMessageAsync(message);
        }
        
        public Tier1And2BalanceEnforcement.FbnAlert.Output SendSMSAcc(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage2 message) {
            return base.Channel.SendSMSAcc(message);
        }
        
        public System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendSMSAccAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage2 message) {
            return base.Channel.SendSMSAccAsync(message);
        }
        
        public Tier1And2BalanceEnforcement.FbnAlert.Output SendMessageGH(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message, Tier1And2BalanceEnforcement.FbnAlert.GHSMSProfile profile, string tranid) {
            return base.Channel.SendMessageGH(message, profile, tranid);
        }
        
        public System.Threading.Tasks.Task<Tier1And2BalanceEnforcement.FbnAlert.Output> SendMessageGHAsync(Tier1And2BalanceEnforcement.FbnAlert.SMSMessage message, Tier1And2BalanceEnforcement.FbnAlert.GHSMSProfile profile, string tranid) {
            return base.Channel.SendMessageGHAsync(message, profile, tranid);
        }
    }
}
