using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.MailKit
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendNewsWithBccAsync(MailRequestForBCC mailRequest);
    }
}
