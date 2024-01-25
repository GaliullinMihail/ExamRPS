namespace RPS.Application.Configs;

public class MongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string MetadataCollectionName { get; set; }
}