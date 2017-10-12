using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace BackupStoreProcessor
{
    public class Functions
    {

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        static TextWriter _log;

        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            var service = new DataConnectionService();
            service.OnDataReceived += Service_OnDataReceived;
            service.GetMessagesFromHub("HostName=training-hub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=+ru85fPh+aYrEhvu0giINpchKDXBjqcciU2P7/RFnm8=");
            log.WriteLine(message);
        }

        private static void Service_OnDataReceived(Microsoft.ServiceBus.Messaging.EventData data)
        {
            Console.WriteLine("Message received {0}", Encoding.UTF8.GetString(data.GetBytes()));
        }
    }
}
