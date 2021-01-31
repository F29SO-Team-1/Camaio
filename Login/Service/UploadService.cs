using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Login.Service
{
    public class UploadService : IUpload
    {
        public CloudBlobContainer GetBlobContainer(string connectionString)
        {
            var storageAcount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAcount.CreateCloudBlobClient();
            return blobClient.GetContainerReference("profile-images");
        }
    }
}
