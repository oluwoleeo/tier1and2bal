﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication1.FbnAlertWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FbnAlertWS.IService")]
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
    public interface IServiceChannel : ConsoleApplication1.FbnAlertWS.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<ConsoleApplication1.FbnAlertWS.IService>, ConsoleApplication1.FbnAlertWS.IService {
        
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
