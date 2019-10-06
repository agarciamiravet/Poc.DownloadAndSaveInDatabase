namespace Poc.DownloadAndSaveInDatabase.Transversal.Configs.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationResult
    {
        private List<string> errorMessages;

        public ValidationResult()
        {
            this.errorMessages = new List<string>();
        }

        public bool Success => !this.errorMessages.Any();

        public IEnumerable<string> ErrorMessages =>  this.errorMessages;
   
        public void CreateErrorMessage(string errorMessage)
        {
            errorMessages.Add(errorMessage);
        }
    }
}
