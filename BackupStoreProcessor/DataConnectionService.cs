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
        public delegate void EventDataReceiver(EventData data);
        public event EventDataReceiver OnDataReceived;

        public void GetMessagesFromHub(string connectionString)
        {
            client = EventHubClient.CreateFromConnectionString(connectionString, "messages/events");
            var partitions = client.GetRuntimeInformation().PartitionIds;
            var taskList = new List<Task>();
            foreach(var item in partitions)
            {
                taskList.Add(receiveMessageFromHub(item));
            }
            Task.WaitAll(taskList.ToArray());
        }

        private async Task receiveMessageFromHub(string partition)
        {
            var receiver = client.GetConsumerGroup("iotdata").CreateReceiver(partition);
            while (true)
            {
                EventData data = await receiver.ReceiveAsync(TimeSpan.FromSeconds(20));
                if(data == null)
                {
                    continue;
                }
                OnDataReceived.Invoke(data);
            }
            

        }
    }
}
