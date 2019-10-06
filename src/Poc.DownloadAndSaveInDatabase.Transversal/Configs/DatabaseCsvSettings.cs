namespace Poc.DownloadAndSaveInDatabase.Transversal.Configs
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs.Validation;

    public class DatabaseCsvSettings : IValidateConfig
    {
        public int BulkBatchSize { get; set; }

        public string CleanDataTableCommand { get; set; }

        public string ConnectionString { get; set; }

        public string DataTable { get; set; }

        public ValidationResult ValidateConfiguration()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(ConnectionString))
            {
                validationResult.CreateErrorMessage("ConnectionString is not configured");
            }

            if (string.IsNullOrEmpty(DataTable))
            {
                validationResult.CreateErrorMessage("DataTable is not configured");
            }

            if (BulkBatchSize == default(int))
            {
                validationResult.CreateErrorMessage("BulkBatchSize is not configured");
            }

            if (string.IsNullOrWhiteSpace(CleanDataTableCommand))
            {
                validationResult.CreateErrorMessage("CleanDataTableCommand is not configured");
            }

            return validationResult;
        }
    }
}
