using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using BackupStoreProcessor.Model;

namespace BackupStoreProcessor
{
    class DataTableService
    {
        private CloudStorageAccount _account;
        private CloudTableClient _tableClient;
        CloudTable _table;

        public void InitConnection(string connectionString, string tableName)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _tableClient = _account.CreateCloudTableClient();
            _table = _tableClient.GetTableReference(tableName);
        }

        public async Task<TableResult> SaveDataToTable(HubMessage data)
        {
            TableOperation insertOperation = TableOperation.Insert(data);
            return await _table.ExecuteAsync(insertOperation);
           
        }
    }
}
