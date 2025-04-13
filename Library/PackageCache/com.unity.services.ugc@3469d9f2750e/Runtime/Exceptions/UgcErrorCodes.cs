// ReSharper disable ConvertToConstant.Global
namespace Unity.Services.Ugc
{
    /// <summary>
    /// UgcErrorCodes lists the error codes to expect from <see cref="UgcException"/> and failed events.
    /// The error code range is: 22000 to 22999.
    /// </summary>
    public static class UgcErrorCodes
    {
        /// <summary>
        /// The minimal value of an UGC error code. Any error code thrown from UGC SDK less than
        /// it is from <see cref="Core.CommonErrorCodes"/>.
        /// </summary>
        public static readonly int MinValue = 22000;

        /// <summary>
        /// The error returned when the stream is either null, empty, not seekable or not readable.
        /// </summary>
        public static readonly int InvalidStream = 22001;

        /// <summary>
        /// The error returned when the client was not authenticated before making the request.
        /// </summary>
        public static readonly int Unauthorized = 22002;

        /// <summary>
        /// The error returned when the request did not return expected results and the result field is null.
        /// </summary>
        public static readonly int NullResults = 22003;

        /// <summary>
        /// Precondition failed.
        /// </summary>
        public static readonly int PreconditionFailed = 22004;

        /// <summary>
        /// The error returned when the content upload failed while sending the data to GCP. It will have an inner exception explaining why the request failed.
        /// </summary>
        public static readonly int UploadContentFailed = 22005;

        /// <summary>
        /// The error returned when the content upload was canceled by the user or due to timeout.
        /// </summary>
        public static readonly int UploadContentCanceled = 22006;

        /// <summary>
        /// The error returned when UGC is enabled for the project but is hidden. To make it visible, visit the Unity Dashboard, UGC > Settings and check the "Is Visible?" checkbox.
        /// </summary>
        public static readonly int ProjectHidden = 22007;

        /// <summary>
        /// The error returned when UGC is not enabled for the project.
        /// </summary>
        public static readonly int ProjectInactive = 22008;

        /// <summary>
        /// The error returned when the content wasn't found.
        /// </summary>
        public static readonly int ContentNotFound = 22009;

        /// <summary>
        /// The error returned when the user isn't the creator of the content.
        /// </summary>
        public static readonly int ContentUnauthorized = 22010;

        /// <summary>
        /// The error returned when the content is hidden from the user.
        /// </summary>
        public static readonly int ContentHidden = 22011;

        /// <summary>
        /// The error returned when the representation wasn't found.
        /// </summary>
        public static readonly int RepresentationNotFound = 22012;

        /// <summary>
        /// The error returned when the tag creation failed.
        /// </summary>
        public static readonly int InvalidTagOperation = 22013;

        /// <summary>
        /// The error returned when the tag creation failed due to an invalid tag name.
        /// </summary>
        public static readonly int InvalidTagName = 22014;

        /// <summary>
        /// The error returned when unknown tag ids were passed to the request.
        /// </summary>
        public static readonly int InvalidTagAssignment = 22015;

        /// <summary>
        /// The error returned when the content wasn't rated yet.
        /// </summary>
        public static readonly int ContentNotRated = 22016;

        /// <summary>
        /// The error returned when the user is already subscribed to the content.
        /// </summary>
        public static readonly int UserAlreadySubscribed = 22017;

        /// <summary>
        /// The error returned when the user is already subscribed to the content.
        /// </summary>
        public static readonly int SubscriptionNotFound = 22018;
    }
}
