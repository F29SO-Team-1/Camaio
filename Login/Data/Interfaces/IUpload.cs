using Microsoft.WindowsAzure.Storage.Blob;

namespace Login.Service
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string blobName);
    }
}
