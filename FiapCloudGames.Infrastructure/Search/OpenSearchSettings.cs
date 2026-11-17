namespace FiapCloudGames.Infrastructure.Search;

public sealed class OpenSearchSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string IndexName { get; set; } = "games-v1";
    public string AwsRegion { get; set; } = "us-east-1";
    public bool UseAwsSigV4 { get; set; }
}
