﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace BackupStoreProcessor.Model
{
    public class HubMessage : TableEntity
    {
        //public DateTime CreatedAt { get; set; }
        public string Entity { get; set; }
    }
}
