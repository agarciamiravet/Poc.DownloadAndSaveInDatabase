using System;
using System.Threading.Tasks;

namespace Poc.DownloadAndSaveInDatabase.Transversal.Interfaces
{
    public interface IBlobStorageDownloadProcessor
    {
        Task<string> DownloadPublicFile(Uri fileUrl, string localPath, string localFile);
    }
}
