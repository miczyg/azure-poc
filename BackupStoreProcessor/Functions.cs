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
        private static DataTableService _dataTable;
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            _dataTable = new DataTableService();
            _dataTable.InitConnection(
                "DefaultEndpointsProtocol=https;AccountName=iotstoragebackup;AccountKey=a3dcqT7uyr8GCcO6/zQlROVlSQmw6/NfB9fGlFBuEaWcdRHKUR0pRVKq/M0yzCuFhDiEB7C/El9IJ3TwecKHDQ==;EndpointSuffix=core.windows.net",
                "iotbackup");

            var service = new DataConnectionService();
            service.OnDataReceived += Service_OnDataReceived;
            service.GetMessagesFromHub("HostName=training-hub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=+ru85fPh+aYrEhvu0giINpchKDXBjqcciU2P7/RFnm8=");
            //log.WriteLine(message);
        }

        private static void Service_OnDataReceived(Microsoft.ServiceBus.Messaging.EventData data)
        {
            var message = new { CreatedAt = DateTime.Now, Message = Encoding.UTF8.GetString(data.GetBytes()) };
            var status = _dataTable.SaveDataToTable(message);
            Console.WriteLine("Message received {0}", status.GetHashCode);
        }
    }
}
