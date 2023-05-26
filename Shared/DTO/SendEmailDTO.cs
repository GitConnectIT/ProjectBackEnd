using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;

public class SendEmailDTO
{
    public int[] ClientIds { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    //public IFormFileCollection? Attachments { get; set; }
}
