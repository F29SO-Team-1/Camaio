using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Login.Service
{
    public class UploadService : IUpload
    {
        public CloudBlobContainer GetBlobContainer(string connectionString, string blobName)
        {
            var storageAcount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAcount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(blobName);
        }
    }
}
