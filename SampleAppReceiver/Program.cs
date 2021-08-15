using Microsoft.Azure.ServiceBus;
using SampleShared.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SampleAppReceiver
{
    class Program
    {
        const string connectionString = "Endpoint=sb://asamplewebservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=x8uYklt6KQevQBN/0oSuCLl3X0oKq2OGD3Uy6tKjAGI=";

        static IQueueClient qClient;

        static async Task Main(string[] args)
        {
            qClient = new QueueClient(connectionString, "personqueue");

            var msgOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                //how many times the service can read from the queue ... if 1 you can basically process 1 message
                MaxConcurrentCalls = 1,
                //wait for a message to be processed before processing another one
                AutoComplete = false
            };

            qClient.RegisterMessageHandler(ProcessMessageAsync, msgOptions);

            Console.ReadKey();

            await qClient.CloseAsync();
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Something went wrong, {args.Exception.Message}");

            return Task.CompletedTask;
        }

        private static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            //deserialize message body
            var jsonBody = Encoding.UTF8.GetString(message.Body);

            var personObj = JsonSerializer.Deserialize<Person>(jsonBody);

            Console.WriteLine($"First Name: {personObj.FirstName}");
            Console.WriteLine($"Last Name: {personObj.LastName}");
            Console.WriteLine($"Email: {personObj.Email}");


            //once a message goes from the queue to the service it has a lock on it, because it is being processed
            //Tell queue that the process is completed successfully
            await qClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
