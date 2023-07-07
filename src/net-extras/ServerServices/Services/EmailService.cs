using System.Globalization;
using System.Reflection;
using FluentEmail.Core;
using ServerServices.Interfaces;

namespace ServerServices.Services;

public class EmailService: IEmailService
{
    private IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail) {
        _fluentEmail = fluentEmail;
    }
    
    
    public async Task SendEmailAsync(string to, string subject, string template, string localizationCode, Object parameters)
    {
        //var c = new CultureInfo("en-US");
        /*var c = CultureInfo.CurrentCulture;
        var r = new RegionInfo(c.LCID);
        string contrySufix = r.TwoLetterISORegionName;*/

        await _fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded("API.EmailTemplates." + template + "-" + localizationCode + ".cshtml",
                parameters,
                typeof(EmailService).GetTypeInfo().Assembly).SendAsync();


        //new { Name = "Bob" },
        //throw new NotImplementedException();
    }
}