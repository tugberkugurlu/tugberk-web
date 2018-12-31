namespace Tugberk.Web
{
    public class BlogSettings
    {
        public string DisqusProjectName { get; set; }
    }

    public static class BlogSettingsExtensions
    {
        public static bool IsDisqusEnabled(this BlogSettings blogSettings) =>
            blogSettings.DisqusProjectName != null;
    }
}
