namespace Poc.DownloadAndSaveInDatabase.Transversal.BlobStorage
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Http;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class BlobStorageDownloadProcessor : IBlobStorageDownloadProcessor
    {
        private readonly BlobStorageSettings blobStorageSettings;
        private readonly GenericHttpClient genericHttpClient;

        public BlobStorageDownloadProcessor(BlobStorageSettings blobStorageSettings, GenericHttpClient genericHttpClient)
        {
            this.blobStorageSettings = blobStorageSettings;
            this.genericHttpClient = genericHttpClient;
        }

        public async Task<string> DownloadPublicFile(Uri fileUrl, string localPath, string localFile)
        {
            if(!Directory.Exists(localPath))
            {
                throw new DirectoryNotFoundException("Directory is not found");
            }

            //1 MB chunk;
            var blobSize = genericHttpClient.GetSizeOFile(fileUrl);

            long blockSize = (this.blobStorageSettings.ChunkBlockSize * 1024 * 1024);

            var fileToCreate = Path.Combine(localPath, localFile);

            CloudBlockBlob blob = new CloudBlockBlob(fileUrl);

            using (FileStream fileStream = new FileStream(fileToCreate, FileMode.Create))
            {
                fileStream.SetLength(blobSize);
            }

            long offset = 0;
            long bytesRemaining = blobSize;


            var blobRequestOptions = this.GetBlobRequestsOptions();
            var operationContext = new OperationContext();
            var accessCondition = new AccessCondition();

            do
            {
                if(operationContext.LastResult == null)
                {
                    Console.WriteLine("Downloading file from blobStorage...");
                }
                else
                {
                    Console.WriteLine("Last partial chunk was {0}", operationContext.LastResult.RequestDate);

                }

                

                var bytesToFetch = Math.Min(blockSize, bytesRemaining);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await blob.DownloadRangeToStreamAsync(target: memoryStream, 
                                                          offset: offset, 
                                                          length: bytesToFetch, 
                                                          accessCondition: accessCondition, 
                                                          options: blobRequestOptions, 
                                                          operationContext: operationContext);
                   
                    memoryStream.Position = 0;
                    var contents = memoryStream.ToArray();
                    using (var fileStream = new FileStream(fileToCreate, FileMode.Open))
                    {
                        fileStream.Position = offset;
                        fileStream.Write(contents, 0, contents.Length);
                    }
                    offset += contents.Length;
                    bytesRemaining -= contents.Length;
                }
            }
            while (bytesRemaining > 0);

            return fileToCreate;
        }


        private BlobRequestOptions GetBlobRequestsOptions()
        {
            var blobRequestOptions = new BlobRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(this.blobStorageSettings.RetryPolicyTimeRetriesInSeconds), this.blobStorageSettings.RetryPolicyMaxAttemps),
                MaximumExecutionTime = TimeSpan.FromMinutes(this.blobStorageSettings.MaximumExecutionTimeInMinutes),
                ServerTimeout = TimeSpan.FromMinutes(this.blobStorageSettings.ServerTimeoutInMinutes)
            };

            return blobRequestOptions;
        }
    }
}
