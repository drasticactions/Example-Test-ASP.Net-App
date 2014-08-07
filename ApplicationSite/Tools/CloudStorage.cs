using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ApplicationSite.Tools
{
    public class CloudStorage
    {
        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _container;
        public CloudStorage(string containerName, bool isPublic)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            this.CreateContainer(containerName, isPublic);
        }

        /// <summary>
        /// Creates Azure container.
        /// If container is created (or already exists), return true.
        /// Else, fail and return false. The handler should deal with the false bool.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="isPublic">If the container should be public.</param>
        /// <returns>A bool.</returns>
        private bool CreateContainer(string containerName, bool isPublic)
        {
            try
            {
                _container = _blobClient.GetContainerReference(containerName);
                _container.CreateIfNotExists();
                if (!isPublic) return true;
                _container.SetPermissions(new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Uploads a file to Azure as a blob.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="path">The path for the file. (Ex. Username)</param>
        /// <returns></returns>
        public bool UploadFile(HttpPostedFileBase file, string path)
        {
            try
            {
                if (file == null) return false;
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(path);
                blockBlob.UploadFromStream(file.InputStream);
                return true;
            }
            catch (Exception)
            {
                // TODO: Add specific error handling.
                return false;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                var blob = this.GetBlob(path);
                blob.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public CloudBlockBlob GetBlob(string path)
        {
            try
            {
                return _container.GetBlockBlobReference(path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MemoryStream GetBlobStream(CloudBlockBlob blob)
        {
            var memoryStream = new MemoryStream();
            blob.DownloadToStream(memoryStream);
            return memoryStream;
        }

    }
}