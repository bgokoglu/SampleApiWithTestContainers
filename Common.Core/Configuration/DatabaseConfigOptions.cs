namespace Common.Core.Configuration
{
    public class DatabaseConfigOptions
    {
        public const string Key = "DatabaseConfig";
        
        public int CommandTimeout { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public bool EnableDetailedErrors { get; set; }
    }
}