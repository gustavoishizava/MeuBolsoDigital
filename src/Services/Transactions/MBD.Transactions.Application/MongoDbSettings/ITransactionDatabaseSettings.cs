namespace MBD.Transactions.Application.MongoDbSettings
{
    public interface ITransactionDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}