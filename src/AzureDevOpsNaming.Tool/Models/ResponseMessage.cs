using AzureNaming.Tool.Helpers;
using System.ComponentModel;

namespace AzureNaming.Tool.Models
{
    public class ResponseMessage
    {
        public MessageTypesEnum Type { get; set; } = MessageTypesEnum.INFORMATION;
        public string Header { get; set; } = "Message";
        public string Message { get; set; } =  String.Empty;
        public string MessageDetails { get; set; } =  String.Empty;
    }
}
