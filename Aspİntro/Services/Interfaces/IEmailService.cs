using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string userName, string url, string html, string content);
    }
}
