using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace BackupStoreProcessor
{
    class DataConnectionService
    {
        EventHubClient client;
        public void GetMessagesFromHub(string connectionString)
        {
            client = EventHubClient.CreateFromConnectionString(connectionString);
            var partitions = client.GetRuntimeInformation().PartitionIds;
            client.GetDefaultConsumerGroup().CreateReceiver("");
        }
    }
}
