using System;

namespace Pcf.Preferences.DataAccess.Caching
{
    public static class PreferenceCacheKeys
    {
        public const string All = "all";

        public static string ById(Guid id)
        {
            return id.ToString("D");
        }
    }
}
