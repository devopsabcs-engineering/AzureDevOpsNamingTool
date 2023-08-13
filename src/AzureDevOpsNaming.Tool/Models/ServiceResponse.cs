﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureNaming.Tool.Models
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = false;
        public dynamic? ResponseObject { get; set; } = null;
        public string ResponseMessage { get; set; } =  String.Empty;
    }
}
