using Unity.Services.Core;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// The entry point of the UGC package. Once initialized, you can use the Instance singleton to implement UGC
    /// features.
    /// </summary>
    public static class UgcService
    {
        static IUgcService s_Instance;

        /// <summary>
        /// The default singleton instance to access the UGC service.
        /// </summary>
        /// <exception cref="ServicesInitializationException">
        /// This exception is thrown if the <code>UnityServices.InitializeAsync()</code>
        /// has not finished before accessing the singleton.
        /// </exception>
        public static IUgcService Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    throw new ServicesInitializationException("Singleton is not initialized. " +
                        "Please call UnityServices.InitializeAsync() to initialize.");
                }

                return s_Instance;
            }

            internal set => s_Instance = value;
        }
    }
}
