using ItConsultations.Business.DataAccess.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ItConsultations.DataAccess.FileAccess;

public class AzureBlobStorage : IFileStorage
{
    private readonly string _connectionString;
    private readonly string _containerName;
    private CloudBlobContainer _container;
    private CloudBlobContainer Container;

    public AzureBlobStorage(string connectionString, string containerName) 
    {
        _connectionString = connectionString;
        _containerName = containerName;
    }

    /*public CloudBlobContainer UploadAsync(string fileName, Stream fileStream)
    {
        var storageAccount = CloudStorageAccount.Parse(_connectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        _container = blobClient.GetContainerReference(_containerName);
        await _container.CreateIfNotExistsAsync();
        
        return container;
    }

    public T GetBlob<T>(string blobName)
    {
        var storageAccount = CloudStorageAccount.Parse(_connectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference(_containerName);
        var blob = container.GetBlockBlobReference(blobName);
        return blob;
    }

    public void SaveBlob(string blobName, Stream fileStream)
    {
        var storageAccount = CloudStorageAccount.Parse(_connectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference(_containerName);
        var blob = container.GetBlockBlobReference(blobName);
        blob.UploadFromStreamAsync(fileStream);
    }

    public void DeleteBlob(string blobName)
    {
        var storageAccount = CloudStorageAccount.Parse(_connectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference(_containerName);
        var blob = container.GetBlockBlobReference(blobName);
        blob.DeleteAsync();
    }*/
}