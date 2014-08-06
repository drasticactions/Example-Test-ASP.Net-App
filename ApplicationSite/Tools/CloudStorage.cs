using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace ApplicationSite.Tools
{
    public class CloudStorage
    {
        private CloudStorageAccount _cloudStorageAccount;

        public CloudStorage()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

    }
}