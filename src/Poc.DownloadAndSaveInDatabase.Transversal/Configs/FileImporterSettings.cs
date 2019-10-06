namespace Poc.DownloadAndSaveInDatabase.Transversal.Configs
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs.Validation;

    public class FileImporterSettings : IValidateConfig
    {
        public string BlobStorageFileUrl { get; set; }

        public string SourceFile { get; set; }

        public string DestinationPath { get; set; }

        public string Separator { get; set; }

  
        public ValidationResult ValidateConfiguration()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(BlobStorageFileUrl))
            {
                validationResult.CreateErrorMessage("BlobStorageFileUrl is not configured");
            }

            if (string.IsNullOrEmpty(SourceFile))
            {
                validationResult.CreateErrorMessage("SourceFile is not configured");
            }

            if (string.IsNullOrEmpty(DestinationPath))
            {
                validationResult.CreateErrorMessage("DestinationPath is not configured");
            }

            if (string.IsNullOrEmpty(Separator))
            {
                validationResult.CreateErrorMessage("Separator is not configured");
            }

            return validationResult;
        }
    }
}
