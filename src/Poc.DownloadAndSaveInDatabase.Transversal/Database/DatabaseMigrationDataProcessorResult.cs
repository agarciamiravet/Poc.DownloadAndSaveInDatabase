namespace Poc.DownloadAndSaveInDatabase.Transversal.Database
{
    using System.Collections.Generic;
    using System.Linq;

    public class DatabaseMigrationDataProcessorResult
    {
        private readonly List<string> messages;

        public DatabaseMigrationDataProcessorResult()
        {
            messages = new List<string>();
        }

        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return messages;
            }
        }

        public bool HasErrors
        {
            get
            {
                return ErrorMessages.Any();
            }
        }

        public void CreateMessage(string message)
        {
            messages.Add(message);
        }
    }
}
