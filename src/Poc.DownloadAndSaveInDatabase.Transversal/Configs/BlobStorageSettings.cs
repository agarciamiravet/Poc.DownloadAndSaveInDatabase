namespace Poc.DownloadAndSaveInDatabase.Transversal.Configs
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs.Validation;

    public class BlobStorageSettings : IValidateConfig
    {
        public int ChunkBlockSize { get; set; }

        public int RetryPolicyTimeRetriesInSeconds { get; set; }

        public int RetryPolicyMaxAttemps { get; set; }

        public int MaximumExecutionTimeInMinutes { get; set; }

        public int ServerTimeoutInMinutes { get; set; }


        public ValidationResult ValidateConfiguration()
        {
            var validationResult = new ValidationResult();

            if ( MaximumExecutionTimeInMinutes == default(int))
            {
                validationResult.CreateErrorMessage("MaximumExecutionTimeInMinutes is not configure");
            }

            if (RetryPolicyTimeRetriesInSeconds == default(int))
            {
                validationResult.CreateErrorMessage("RetryPolicyTimeRetriesInSeconds is not configure");
            }

            if (RetryPolicyMaxAttemps == default(int))
            {
                validationResult.CreateErrorMessage("RetryPolicyTimeBetweenRetriesInSeconds is not configure");
            }

            if (ServerTimeoutInMinutes == default(int))
            {
                validationResult.CreateErrorMessage("ServerTimeoutInMinutes is not configure");
            }

            if (ChunkBlockSize == default(int))
            {
                validationResult.CreateErrorMessage("ChunkBlockSize is not configure");
            }

            return validationResult;
        }
    }
}