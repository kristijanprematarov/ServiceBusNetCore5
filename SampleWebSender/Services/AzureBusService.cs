using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using SampleShared.Models;
using SampleWebSender.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleWebSender.Services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IConfiguration _config;

        public AzureBusService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            //Get the connection string from the settings
            var connectionString = _config.GetConnectionString("AzureServiceBusConnection");

            //Initializing the queue
            var qClient = new QueueClient(connectionString, queueName);

            //convert the object into json
            var messageBody = JsonSerializer.Serialize(personMessage);

            //Initialize the Queue message
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            //SEND MESSAGE
            await qClient.SendAsync(message);
        }
    }
}
