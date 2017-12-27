using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using BackupStoreProcessor.Model;

namespace BackupStoreProcessor
{
    public class Functions
    {
        private static DataTableService _dataTable;
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            _dataTable = new DataTableService();
            _dataTable.InitConnection(
                "iotbackup");

            var service = new DataConnectionService();
            service.OnDataReceived += Service_OnDataReceived;
            service.GetMessagesFromHub()
            log.WriteLine(message);
        }

        private static async void Service_OnDataReceived(Microsoft.ServiceBus.Messaging.EventData data)
        {
            var message = new HubMessage
            {
                //CreatedAt = DateTime.Now,
                Entity = Encoding.UTF8.GetString(data.GetBytes()),
                PartitionKey = "metrics",
                RowKey = Guid.NewGuid().ToString()
            };
            var status = await _dataTable.SaveDataToTable(message);
            Console.WriteLine("Message received {0}", status.HttpStatusCode);
        }
    }
}
