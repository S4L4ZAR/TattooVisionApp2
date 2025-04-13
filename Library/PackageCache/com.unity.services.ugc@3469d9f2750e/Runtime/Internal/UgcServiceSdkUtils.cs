using Unity.Services.Core.Configuration.Internal;

namespace Unity.Services.Ugc.Internal
{
    /// <summary>
    /// Utility class for UGC service related features
    /// </summary>
    public static class UgcServiceSdkUtils
    {
        const string k_CloudEnvironmentKey = "com.unity.services.core.cloud-environment";
        const string k_StagingEnvironment = "staging";

        /// <summary>
        /// Set the base url to the UGC Service.
        /// </summary>
        /// <param name="path">The base URL of the UGC Service</param>
        /// <remarks>This is used by Unity internal teams.</remarks>
        public static void SetBasePath(string path)
        {
            if (UgcService.Instance is WrappedUgcService service)
            {
                service.Configuration.BasePath = path;
            }
        }

        internal static string GetHost(IProjectConfiguration projectConfiguration)
        {
            var cloudEnvironment = projectConfiguration?.GetString(k_CloudEnvironmentKey);

            switch (cloudEnvironment)
            {
                case k_StagingEnvironment:
                    return "https://ugc-stg.services.api.unity.com";
                default:
                    return "https://ugc.services.api.unity.com";
            }
        }
    }
}
