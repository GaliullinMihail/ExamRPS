namespace RPS.Application.Configs;

public class MongoDbConfig
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string MetadataCollectionName { get; set; } = default!;
}