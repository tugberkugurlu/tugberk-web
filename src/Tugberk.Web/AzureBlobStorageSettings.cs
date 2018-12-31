namespace Tugberk.Web
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; }
    }

    public static class AzureBlobStorageSettingsExtensions
    {
        public static bool IsAzureBlobStorageEnabled(this AzureBlobStorageSettings azureBlobStorageSettings) =>
            azureBlobStorageSettings.ConnectionString != null;
    }
}
