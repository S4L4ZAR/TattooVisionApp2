using System;
using Unity.Services.Core;
using Unity.Services.Ugc.Generated.Http;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// UgcException represents a runtime exception from UGC.
    /// </summary>
    public class UgcException : RequestFailedException
    {
        /// <summary>
        /// Constructor of the UgcException with the error code, a message, and inner exception.
        /// </summary>
        /// <param name="errorCode">The error code for UgcException.</param>
        /// <param name="message">The additional message that helps to debug the error.</param>
        /// <param name="innerException">The inner exception reference.</param>
        UgcException(int errorCode, string message, Exception innerException = null)
            : base(errorCode, message, innerException)
        {
        }

        /// <summary>
        /// Creates the exception base on errorCode range.
        /// If the errorCode is less than UgcErrorCodes.MinValue it creates a <see cref="RequestFailedException"/>.
        /// Otherwise it creates an <see cref="UgcException"/>
        /// </summary>
        internal static RequestFailedException Create(int errorCode, string message, Exception innerException = null)
        {
            if (errorCode < UgcErrorCodes.MinValue)
            {
                return new RequestFailedException(errorCode, message, innerException);
            }

            return new UgcException(errorCode, message, innerException);
        }

        internal static RequestFailedException HandleUnknownException(Exception exception)
        {
            const string message = "An unknown error occurred in the UGC SDK.";
            return Create(CommonErrorCodes.Unknown, message, exception);
        }

        internal static RequestFailedException HandleDeserializationException(ResponseDeserializationException exception)
        {
            const string message = "A deserialization exception occurred in the UGC SDK. Check the inner exception.";
            return Create(CommonErrorCodes.Unknown, message, exception);
        }
    }
}
