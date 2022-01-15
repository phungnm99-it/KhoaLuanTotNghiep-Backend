using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.MailKit
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class MailRequestForBCC
    {
        public string Subject { get; set; }
        public List<string> ToBcc { get; set; }
        public string Body { get; set; }
    }
}
