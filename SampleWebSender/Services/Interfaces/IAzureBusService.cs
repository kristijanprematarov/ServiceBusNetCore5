using SampleShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebSender.Services.Interfaces
{
    public interface IAzureBusService
    {
        Task SendMessageAsync(Person personMessage, string queueName);
    }
}
