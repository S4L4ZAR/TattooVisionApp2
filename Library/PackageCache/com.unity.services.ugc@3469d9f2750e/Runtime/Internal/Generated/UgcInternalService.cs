using Unity.Services.Ugc.Generated;
using UnityEngine;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// Small wrapper for generated code
    /// </summary>
    static class UgcInternalService
    {
        internal static IServiceConfiguration Instance = null;
    }

    interface IServiceConfiguration
    {
        Configuration Configuration { get; }
    }
}
