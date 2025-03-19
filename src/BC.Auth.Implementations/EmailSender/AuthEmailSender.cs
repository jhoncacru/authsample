using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Auth.Implementations.EmailSender;

public class AuthEmailSender : IEmailSender
{
    private readonly ILogger<AuthEmailSender> _logger;

    public AuthEmailSender(ILogger<AuthEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("sending email to "+email+" - "+subject);
        return Task.CompletedTask;
    }
}
