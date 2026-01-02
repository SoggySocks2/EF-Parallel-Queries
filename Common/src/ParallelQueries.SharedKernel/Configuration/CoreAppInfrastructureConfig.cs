namespace ParallelQueries.SharedKernel.Configuration;

public class CoreAppInfrastructureConfig
{
    public const string CONFIG_NAME = "CoreAppInfrastructure";

    public static CoreAppInfrastructureConfig Instance { get; } = new CoreAppInfrastructureConfig();
    private CoreAppInfrastructureConfig() { }
    public string ConnectionString { get; set; } = string.Empty;
}
