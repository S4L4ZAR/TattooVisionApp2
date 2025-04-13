using System.IO;
using Unity.Services.Core;
using Unity.Services.Ugc.Generated;

namespace Unity.Services.Ugc.Internal
{
    static class UgcValidators
    {
        internal static void ValidateSuccessfulStatusCode(long statusCode, long expectedStatusCode = 200)
        {
            if (statusCode != expectedStatusCode)
                throw UgcException.Create(CommonErrorCodes.Unknown, $"Unexpected statusCode {statusCode} instead of {expectedStatusCode} from the service");
        }

        internal static void ValidateStream(Stream stream)
        {
            if (stream == null || !stream.CanSeek || !stream.CanRead || stream.Length == 0)
                throw UgcException.Create(UgcErrorCodes.InvalidStream, $"{stream} is in an invalid state - make sure the stream is non-null, readable and seekable.");
        }

        internal static void ValidateResult<T>(Response<T> response)
        {
            if (response.Result == null)
            {
                throw UgcException.Create(UgcErrorCodes.NullResults, "No results - request did not return anything, please check the request arguments and try again.");
            }
        }

        internal static void CheckForClientServerErrorCode(long statusCode)
        {
            if (statusCode < 400)
                return;

            switch (statusCode)
            {
                case 400:
                    throw UgcException.Create(CommonErrorCodes.InvalidRequest, "Invalid Request - Please check the request's parameters and try again.");
                case 401:
                    throw UgcException.Create(UgcErrorCodes.Unauthorized, "Unauthorized - Make sure you are signed in and try again.");
                case 403:
                    throw UgcException.Create(CommonErrorCodes.Forbidden, "Forbidden - Make sure you have the required permissions to make this request.");
                case 404:
                    throw UgcException.Create(CommonErrorCodes.NotFound, "Not Found - The server could not find the requested resource, please make sure it exists, then try again.");
                case 412:
                    throw UgcException.Create(UgcErrorCodes.PreconditionFailed,
                        "Precondition failed.  Try again.");
                case 429:
                    throw UgcException.Create(CommonErrorCodes.TooManyRequests, "Too Many Requests - Too many requests have been sent, please try again later.");
                case 500:
                case 503:
                    throw UgcException.Create(CommonErrorCodes.ServiceUnavailable, "The service is unavailable - please try again later.");
                default:
                    throw UgcException.Create(CommonErrorCodes.Unknown, $"Unknown status code {statusCode} - please try again later.");
            }
        }
    }
}
