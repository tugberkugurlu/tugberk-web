namespace Tugberk.Web
{
    public static class BlogSettingsExtensions
    {
        public static bool IsDisqusEnabled(this BlogSettings blogSettings) =>
            blogSettings.DisqusProjectName != null;
    }
}
