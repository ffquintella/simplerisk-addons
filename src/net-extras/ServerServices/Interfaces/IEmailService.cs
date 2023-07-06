﻿
using System.Reflection;

namespace ServerServices.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string template, Object parameters, Assembly templatesAssembly);
    
}