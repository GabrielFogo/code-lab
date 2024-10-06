using System.Security.Authentication;
using MongoDB.Driver;

namespace CodeLab.Data
{
    public class ContextMongoDb
    {
        public static string? ConnectionString { get; set; }
        public static string? DatabaseName { get; set; }
        public static bool IsSsl { get; set; }
        private IMongoDatabase? _database;

        public ContextMongoDb()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException(nameof(ConnectionString), "Connection string cannot be null or empty.");
                
            if (string.IsNullOrEmpty(DatabaseName))
                throw new ArgumentNullException(nameof(DatabaseName), "Database name cannot be null or empty.");

            try
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

                if (IsSsl)
                {
                    settings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = SslProtocols.Tls12
                    };
                }

                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível conectar", ex);
            }
        }

        public IMongoDatabase? GetDatabase() => _database;
    }
}