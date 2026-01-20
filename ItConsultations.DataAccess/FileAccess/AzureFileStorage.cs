using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace ItConsultations.DataAccess.FileAccess;

public class AzureFileStorage
{
    private readonly CloudStorageAccount _storageAccount;

    private readonly CloudFileClient _cloudFileClient;

    private readonly CloudFileDirectory _rootDirectory;
}
