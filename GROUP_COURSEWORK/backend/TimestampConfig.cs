using System;

namespace backend
{
    public static class TimestampConfig
    {
        public static void Initialize()
        {
            // Enable legacy timestamp behavior for Npgsql
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
