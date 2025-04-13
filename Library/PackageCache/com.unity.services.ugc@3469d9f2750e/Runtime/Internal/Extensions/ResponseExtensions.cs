using Unity.Services.Ugc.Generated;

namespace Unity.Services.Ugc.Internal
{
    static class ResponseExtensions
    {
        internal static void EnsureUgcSuccessStatusCode(this Response response, long expectedStatusCode = 200)
        {
            UgcValidators.CheckForClientServerErrorCode(response.Status);
            UgcValidators.ValidateSuccessfulStatusCode(response.Status, expectedStatusCode);
        }
    }
}
