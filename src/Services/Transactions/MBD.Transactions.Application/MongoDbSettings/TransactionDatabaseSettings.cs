namespace MBD.Transactions.Application.MongoDbSettings
{
    public class TransactionDatabaseSettings : ITransactionDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}