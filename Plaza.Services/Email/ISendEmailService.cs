using Line.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Services
{
    public partial interface  ISendEmailService
    {
        void SendEmail(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
             string replyTo = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0, IDictionary<string, string> headers = null);
    }
}
