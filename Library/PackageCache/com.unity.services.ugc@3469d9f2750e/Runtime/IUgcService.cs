using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Core;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// This interface describes the UGC related methods available to game clients.
    /// </summary>
    public interface IUgcService
    {
        /// <summary>
        /// Get all content from a project specific environment.
        /// Content with visibility set to Hidden or with ModerationStatus different from Approved won't be returned.
        /// Deleted contents and ones that haven't finished uploading won't be returned either.
        /// </summary>
        /// <param name="getContentsArgs">The details of the search request</param>
        /// <returns>A list of contents from the environment with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Content>> GetContentsAsync(GetContentsArgs getContentsArgs = null);

        /// <summary>
        /// Get all content created by the current signed in player
        /// </summary>
        /// <param name="getPlayerContentsArgs">The details of the search request</param>
        /// <returns>A list of contents from the environment with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Content>> GetPlayerContentsAsync(GetPlayerContentsArgs getPlayerContentsArgs = null);

        /// <summary>
        /// Get content by <see cref="ContentTrendType"/>
        /// </summary>
        /// <param name="getContentTrendsArgs">The details of the search request</param>
        /// <returns>A list of contents of this trend type, with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Content>> GetContentTrendsAsync(GetContentTrendsArgs getContentTrendsArgs = null);

        /// <summary>
        /// Get content versions of a content.
        /// </summary>
        /// <param name="getContentVersionsArgs">Contains all the parameters of the request</param>
        /// <returns>The list of versions associated with the content with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<ContentVersion>> GetContentVersionsAsync(GetContentVersionsArgs getContentVersionsArgs);

        /// <summary>
        /// Get a specific content.
        /// </summary>
        /// <param name="getContentArgs">Contains all the parameters of the request</param>
        /// <returns>The requested content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ProjectHidden"/> if the project isn't onboarded/active. Check if the project is linked and enabled in the dashboard.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentUnauthorized"/> if the current user isn't the creator of the content and its visibility is private.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> GetContentAsync(GetContentArgs getContentArgs);

        /// <summary>
        /// Create a new content in a project environment
        /// </summary>
        /// <param name="createContentArgs">Contains all the parameters of the request</param>
        /// <returns>The content that will be eventually available once the upload has been uploaded.</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidStream"/> if asset or thumbnail streams are in invalid states.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.UploadContentFailed"/> if asset and/or thumbnail uploads requests failed. Check inner exceptions for more details</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidTagAssignment"/> if unknown tag ids were passed to the request.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> CreateContentAsync(CreateContentArgs createContentArgs);

        /// <summary>
        /// Create a new version of a content.
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <param name="asset">The stream containing the binary payload of the content</param>
        /// <param name="thumbnail">The stream containing the image representing the content</param>
        /// <returns>The content that will be eventually available once the upload has been uploaded.</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidStream"/> if asset or thumbnail streams are in invalid states.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.UploadContentFailed"/> if asset and/or thumbnail uploads requests failed. Check inner exceptions for more details</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentUnauthorized"/> if the current user isn't the creator of the content.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> CreateContentVersionAsync(string contentId, Stream asset, Stream thumbnail = null);

        /// <summary>
        /// Delete a content
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>A task</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentUnauthorized"/> if the current user isn't the creator of the content.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task DeleteContentAsync(string contentId);

        /// <summary>
        /// Get a list of tags associated with the project
        /// </summary>
        /// <returns>A list of tags</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<List<Tag>> GetTagsAsync();

        /// <summary>
        /// Get the rating of a content.
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>The rating of the content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotRated"/> if the content doesn't have ratings yet.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<ContentUserRating> GetUserContentRatingAsync(string contentId);

        /// <summary>
        /// Submit a user rating of a content
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <param name="rating">The rating value</param>
        /// <returns>The rating</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<ContentUserRating> SubmitUserContentRatingAsync(string contentId, float rating);

        /// <summary>
        /// Report a content
        /// </summary>
        /// <param name="reportContentArgs">The details of the report request</param>
        /// <returns>The reported content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ProjectInactive"/> if the project was deleted.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> ReportContentAsync(ReportContentArgs reportContentArgs);

        /// <summary>
        /// Approve content that needed moderation.
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>The approved content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ProjectInactive"/> if the project was deleted.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> ApproveContentAsync(string contentId);

        /// <summary>
        /// Reject content that needed moderation.
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>The rejected content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ProjectInactive"/> if the project was deleted.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> RejectContentAsync(string contentId);

        /// <summary>
        /// Search for content that needs moderation in the project.
        /// </summary>
        /// <param name="searchContentModerationArgs">The details of the search request</param>
        /// <returns>A list of contents requiring moderation with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Content>> SearchContentModerationAsync(SearchContentModerationArgs searchContentModerationArgs);

        /// <summary>
        /// Update a specific content details.
        /// </summary>
        /// <param name="updateContentDetailsArgs">The details of the update request</param>
        /// <returns>The updated content</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentUnauthorized"/> if the current user isn't the creator of the content.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidTagAssignment"/> if unknown tag ids were passed to the request.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Content> UpdateContentDetailsAsync(UpdateContentDetailsArgs updateContentDetailsArgs);

        /// <summary>
        /// Subscribe to the content for the current user
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>The created subscription</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.UserAlreadySubscribed"/> if the user is already subscribed to the content.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Subscription> CreateSubscriptionAsync(string contentId);

        /// <summary>
        /// Unsubscribe to the content for the current user
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>A task</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task DeleteSubscriptionAsync(string contentId);

        /// <summary>
        /// Get all subscriptions of the current user
        /// </summary>
        /// <param name="getSubscriptionsArgs">The details of the search request</param>
        /// <returns>A list of subscriptions of the current user with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Subscription>> GetSubscriptionsAsync(GetSubscriptionsArgs getSubscriptionsArgs);

        /// <summary>
        /// Check if the current user is subscribed to the content
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <returns>true if the content is subscribed to, false otherwise</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.ContentNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<bool> IsSubscribedToAsync(string contentId);

        /// <summary>
        /// Download data and/or thumbnail of a content
        /// </summary>
        /// <param name="content">The content that will have its data downloaded</param>
        /// <param name="downloadContent">True if we want to download the content's data</param>
        /// <param name="downloadThumbnail">True if we want to download the content's thumbnail</param>
        /// <returns>The downloaded data will be put in the `content` parameter so this call returns nothing.</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task DownloadContentDataAsync(Content content, bool downloadContent, bool downloadThumbnail);

        /// <summary>
        /// Create a new representation of a content
        /// </summary>
        /// <param name="createRepresentationArgs">Contains all the parameters of the request</param>
        /// <returns>The created representation</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidTagAssignment"/> if unknown tag ids were passed to the request.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Representation> CreateRepresentationAsync(CreateRepresentationArgs createRepresentationArgs);

        /// <summary>
        /// Get a specific representation of a content
        /// </summary>
        /// <param name="getRepresentationArgs">Contains all the parameters of the request</param>
        /// <returns>The representation</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Representation> GetRepresentationAsync(GetRepresentationArgs getRepresentationArgs);

        /// <summary>
        /// Update a specific representation.
        /// </summary>
        /// <param name="updateRepresentationArgs">The details of the update request</param>
        /// <returns>The updated representation</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.RepresentationNotFound"/> if the API call failed due requested representation not being found.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidTagName"/> if invalid tag names were passed to the request.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<Representation> UpdateRepresentationAsync(UpdateRepresentationArgs updateRepresentationArgs);

        /// <summary>
        /// Create a representation version.
        /// </summary>
        /// <param name="contentId">The content identifier</param>
        /// <param name="representationId">The representation identifier</param>
        /// <param name="asset">The stream containing the binary payload of the representation version</param>
        /// <returns>The representation.</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.InvalidStream"/> if asset or thumbnail streams are in invalid states.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.UploadContentFailed"/> if asset and/or thumbnail uploads requests failed. Check inner exceptions for more details</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.RepresentationNotFound"/> if the API call failed due requested content not being found.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<RepresentationVersion> CreateRepresentationVersionAsync(string contentId, string representationId,
            Stream asset);


        /// <summary>
        /// Get representations of a content
        /// </summary>
        /// <param name="getRepresentationsArgs">The details of the search request</param>
        /// <returns>A list of representations of specific content with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Representation>> GetRepresentationsAsync(GetRepresentationsArgs getRepresentationsArgs);

        /// <summary>
        /// Search representations within a project
        /// </summary>
        /// <param name="searchRepresentationsArgs">The details of the search request</param>
        /// <returns>A list of representations within a project with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<Representation>> SearchRepresentationsAsync(SearchRepresentationsArgs searchRepresentationsArgs);

        /// <summary>
        /// Get versions of a representation
        /// </summary>
        /// <param name="getRepresentationVersionsArgs">The details of the search request</param>
        /// <returns>A list of versions of a specific representation with pagination information</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.NullResults"/> if the request did not return any results. Check the request arguments.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task<PagedResults<RepresentationVersion>> GetRepresentationVersionsAsync(GetRepresentationVersionsArgs getRepresentationVersionsArgs);

        /// <summary>
        /// Delete representation
        /// </summary>
        /// <param name="deleteRepresentationArgs">The id of the representation to delete</param>
        /// <returns>A task</returns>
        /// <exception cref="UgcException">
        /// Thrown if request is unsuccessful due to UGC Service specific issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="UgcErrorCodes.Unauthorized"/> if the user is not authorized to perform this operation. Check if the project is linked and the user correctly authenticated.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown if request is unsuccessful due to common services issues.
        /// <list type="bullet">
        /// <item><description>Throws with <c>ErrorCode</c> <see cref="CommonErrorCodes.Unknown"/> if the API call failed due to unexpected response from the server.</description></item>
        /// </list>
        /// </exception>
        Task DeleteRepresentationAsync(DeleteRepresentationArgs deleteRepresentationArgs);
    }
}
