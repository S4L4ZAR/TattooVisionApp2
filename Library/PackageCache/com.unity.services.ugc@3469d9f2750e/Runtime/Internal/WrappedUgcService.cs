using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication.Internal;
using Unity.Services.Core;
using Unity.Services.Core.Configuration.Internal;
using Unity.Services.Ugc.Generated;
using Unity.Services.Ugc.Generated.Apis.Content;
using Unity.Services.Ugc.Generated.Apis.ContentVersions;
using Unity.Services.Ugc.Generated.Apis.Moderation;
using Unity.Services.Ugc.Generated.Apis.Representation;
using Unity.Services.Ugc.Generated.Apis.Subscription;
using Unity.Services.Ugc.Generated.Apis.Tag;
using Unity.Services.Ugc.Generated.Content;
using Unity.Services.Ugc.Generated.ContentVersions;
using Unity.Services.Ugc.Generated.Http;
using Unity.Services.Ugc.Generated.Models;
using Unity.Services.Ugc.Generated.Representation;
using Unity.Services.Ugc.Generated.Subscription;
using Unity.Services.Ugc.Generated.Tag;
using CreateRepresentationRequest = Unity.Services.Ugc.Generated.Representation.CreateRepresentationRequest;
using HttpClient = System.Net.Http.HttpClient;
using RejectContentRequest = Unity.Services.Ugc.Generated.Moderation.RejectContentRequest;
using ReportContentRequest = Unity.Services.Ugc.Generated.Moderation.ReportContentRequest;
using UpdateRepresentationRequest = Unity.Services.Ugc.Generated.Representation.UpdateRepresentationRequest;

namespace Unity.Services.Ugc.Internal
{
    interface IContentHttpClient
    {
        Task<HttpResponseMessage> SendGetRequestAsync(string url, int requestTimeout, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> SendPutRequestAsync(string url, Stream bodyStream, IDictionary<string, List<string>> uploadContentHeaders, int requestTimeout, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// This class allows a game client to interact with the Unity UGC Service.
    /// </summary>
    class WrappedUgcService : IUgcService, IServiceConfiguration
    {
        public Configuration Configuration { get; }

        readonly IAccessToken m_AccessTokenComponent;
        readonly ICloudProjectId m_CloudProjectIdComponent;
        readonly IEnvironmentId m_EnvironmentIdComponent;

        readonly IContentApiClient m_ContentApiClient;
        readonly IContentVersionsApiClient m_ContentVersionsApiClient;
        readonly ITagApiClient m_TagApiClient;
        readonly ISubscriptionApiClient m_SubscriptionApiClient;
        readonly IRepresentationApiClient m_RepresentationApiClient;
        readonly IModerationApiClient m_ModerationApiClient;
        readonly IContentHttpClient m_ContentHttpClient;
        readonly MD5 m_MD5;

        string ProjectId => m_CloudProjectIdComponent.GetCloudProjectId();
        string EnvironmentId => m_EnvironmentIdComponent.EnvironmentId;

        readonly Task<byte[]> m_CompletedDownload = Task.FromResult<byte[]>(null);

        internal WrappedUgcService(IContentApiClient contentApiClient,
                                   ITagApiClient tagApiClient,
                                   ISubscriptionApiClient subscriptionApiClient,
                                   IContentVersionsApiClient contentVersionsApiClient,
                                   IRepresentationApiClient representationApiClient,
                                   IModerationApiClient moderationApiClient,
                                   IContentHttpClient contentHttpClient,
                                   IAccessToken accessTokenComponent,
                                   ICloudProjectId cloudProjectIdComponent,
                                   IEnvironmentId environmentIdComponent,
                                   Configuration configuration)
        {
            m_ContentApiClient = contentApiClient;
            m_ContentVersionsApiClient = contentVersionsApiClient;
            m_TagApiClient = tagApiClient;
            m_SubscriptionApiClient = subscriptionApiClient;
            m_RepresentationApiClient = representationApiClient;
            m_ModerationApiClient = moderationApiClient;
            m_ContentHttpClient = contentHttpClient;

            m_AccessTokenComponent = accessTokenComponent;
            m_CloudProjectIdComponent = cloudProjectIdComponent;
            m_EnvironmentIdComponent = environmentIdComponent;

            Configuration = configuration;
            m_MD5 = MD5.Create();
        }

        public async Task<PagedResults<Content>> GetContentsAsync(GetContentsArgs getContentsArgs = null)
        {
            var requestArgs = getContentsArgs ?? new GetContentsArgs();
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_ContentApiClient.SearchContentAsync(
                new SearchContentRequest(ProjectId, EnvironmentId, requestArgs.Offset, requestArgs.Limit,
                    requestArgs.SortBys, requestArgs.Search, requestArgs.Tags, requestArgs.Filters, requestArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Content(x));
            return new PagedResults<Content>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<PagedResults<Content>> GetPlayerContentsAsync(GetPlayerContentsArgs getPlayerContentsArgs = null)
        {
            var requestArgs = getPlayerContentsArgs ?? new GetPlayerContentsArgs();
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_ContentApiClient.SearchPlayerContentAsync(
                new SearchPlayerContentRequest(requestArgs.Offset, requestArgs.Limit,
                    requestArgs.SortBys, requestArgs.Search, requestArgs.Filters, requestArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Content(x));
            return new PagedResults<Content>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<PagedResults<ContentVersion>> GetContentVersionsAsync(GetContentVersionsArgs getContentVersionsArgs)
        {
            var requestArgs = getContentVersionsArgs;
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_ContentVersionsApiClient.SearchContentVersionsAsync(
                new SearchContentVersionsRequest(ProjectId, EnvironmentId, requestArgs.ContentId, requestArgs.Offset,
                    requestArgs.Limit, requestArgs.SortBys, requestArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new ContentVersion(x));
            return new PagedResults<ContentVersion>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<PagedResults<Content>> GetContentTrendsAsync(GetContentTrendsArgs getContentTrendsArgs = null)
        {
            var requestArgs = getContentTrendsArgs ?? new GetContentTrendsArgs(ContentTrendType.TopRated);
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_ContentApiClient.GetContentTrendsListAsync(
                new GetContentTrendsListRequest(ProjectId, EnvironmentId, requestArgs.TrendType.ToString(), requestArgs.Offset,
                    requestArgs.Limit, requestArgs.GetSortBy(), requestArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Content(x));
            return new PagedResults<Content>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<Content> CreateContentAsync(CreateContentArgs createContentArgs)
        {
            var uploadContentResponse = await ExecuteAsyncWithReturn(() =>
            {
                UgcValidators.ValidateStream(createContentArgs.Asset);
                var assetMd5Hash = GetHash(m_MD5, createContentArgs.Asset);
                string thumbnailMd5Hash = null;
                if (createContentArgs.Thumbnail != null)
                {
                    UgcValidators.ValidateStream(createContentArgs.Thumbnail);
                    thumbnailMd5Hash = GetHash(m_MD5, createContentArgs.Thumbnail);
                }

                return m_ContentApiClient.CreateContentAsync(
                    new CreateContentRequest(ProjectId, EnvironmentId, new NewContentRequest(createContentArgs.Name,
                        createContentArgs.Description, createContentArgs.CustomId,
                        createContentArgs.IsPublic ? ContentVisibility.Public : ContentVisibility.Private,
                        createContentArgs.TagsId, assetMd5Hash, thumbnailMd5Hash, createContentArgs.Metadata)),
                    Configuration);
            });

            try
            {
                await SendUploadContentRequest(uploadContentResponse, createContentArgs.Asset,
                    createContentArgs.Thumbnail);
            }
            catch (RequestFailedException e)
            {
                throw UgcException.Create(UgcErrorCodes.UploadContentFailed,
                    "Upload to GCP failed - could not send the content to GCP, see inner exception for details", e);
            }

            return new Content(uploadContentResponse.Content);
        }

        public async Task<Content> CreateContentVersionAsync(string contentId, Stream asset, Stream thumbnail = null)
        {
            var maxRetryAttempts = Configuration.NumberOfRetries.GetValueOrDefault();
            var currentAttempt = 0;

            UgcValidators.ValidateStream(asset);
            var assetMd5Hash = GetHash(m_MD5, asset);

            string thumbnailMd5Hash = null;
            if (thumbnail != null)
            {
                UgcValidators.ValidateStream(thumbnail);
                thumbnailMd5Hash = GetHash(m_MD5, thumbnail);
            }

            do
            {
                try
                {
                    var uploadContentResponse = await ExecuteAsyncWithReturn(() =>
                        m_ContentApiClient.CreateContentVersionAsync(
                            new CreateContentVersionRequest(ProjectId, EnvironmentId, contentId,
                                new AddVersionRequest(assetMd5Hash, thumbnailMd5Hash)),
                            Configuration));

                    await SendUploadContentRequest(uploadContentResponse, asset, thumbnail);
                    return new Content(uploadContentResponse.Content, uploadContentResponse.Version);
                }
                catch (RequestFailedException e)
                {
                    if (e.ErrorCode != UgcErrorCodes.PreconditionFailed)
                    {
                        throw UgcException.Create(UgcErrorCodes.UploadContentFailed,
                            "Upload to GCP failed - could not send the content to GCP, see inner exception for details",
                            e);
                    }

                    currentAttempt++;
                }
            }
            while (currentAttempt < maxRetryAttempts);

            throw UgcException.Create(UgcErrorCodes.UploadContentFailed,
                "Upload to GCP failed - could not send the content to GCP, see inner exception for details");
        }

        public async Task<Content> UpdateContentDetailsAsync(UpdateContentDetailsArgs updateContentDetailsArgs)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ContentApiClient.UpdateDetailsAsync(
                new UpdateDetailsRequest(ProjectId, EnvironmentId, updateContentDetailsArgs.ContentId,
                    new UpdateContentRequest(updateContentDetailsArgs.Name, updateContentDetailsArgs.Description, updateContentDetailsArgs.CustomId,
                        updateContentDetailsArgs.IsPublic ? ContentVisibility.Public : ContentVisibility.Private,
                        0, updateContentDetailsArgs.TagsId, updateContentDetailsArgs.Version, updateContentDetailsArgs.Metadata)), Configuration));
            return new Content(result);
        }

        public async Task DeleteContentAsync(string contentId)
        {
            await ExecuteAsync(() => m_ContentApiClient.DeleteContentAsync(new DeleteContentRequest(ProjectId,
                EnvironmentId, contentId), Configuration), 204);
        }

        public async Task<Content> GetContentAsync(GetContentArgs getContentArgs)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ContentApiClient.GetContentAsync(new GetContentRequest(ProjectId, EnvironmentId,
                getContentArgs.ContentId, getContentArgs.IncludeStatistics), Configuration));

            var content = new Content(result);

            await DownloadContentDataAsync(content, getContentArgs.DownloadContent, getContentArgs.DownloadThumbnail);

            return content;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var result = await ExecuteAsyncWithReturn(() => m_TagApiClient.GetTagsAsync(new GetTagsRequest(ProjectId, EnvironmentId),
                Configuration));
            var results = result.ConvertAll(x => new Tag(x));
            return results;
        }

        public async Task<ContentUserRating> GetUserContentRatingAsync(string contentId)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ContentApiClient.GetUserRatingAsync(
                new GetUserRatingRequest(ProjectId, EnvironmentId, contentId), Configuration));
            return new ContentUserRating(result);
        }

        public async Task<ContentUserRating> SubmitUserContentRatingAsync(string contentId, float rating)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ContentApiClient.CreateUserRatingAsync(
                new CreateUserRatingRequest(ProjectId, EnvironmentId, contentId, new ContentRatingRequest(rating)),
                Configuration));
            return new ContentUserRating(result);
        }

        public async Task<Content> ReportContentAsync(ReportContentArgs reportContentArgs)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ModerationApiClient.ReportContentAsync(new ReportContentRequest(ProjectId, EnvironmentId,
                reportContentArgs.ContentId, new Generated.Models.ReportContentRequest(reportContentArgs.ReportReason, reportContentArgs.OtherReason)), Configuration));
            return new Content(result);
        }

        public async Task<Content> ApproveContentAsync(string contentId)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ModerationApiClient.ApproveContentAsync(new Generated.Moderation.ApproveContentRequest(ProjectId, EnvironmentId,
                contentId), Configuration));
            return new Content(result);
        }

        public async Task<Content> RejectContentAsync(string contentId)
        {
            var result = await ExecuteAsyncWithReturn(() => m_ModerationApiClient.RejectContentAsync(new RejectContentRequest(ProjectId, EnvironmentId,
                contentId), Configuration));
            return new Content(result);
        }

        public async Task<PagedResults<Content>> SearchContentModerationAsync(SearchContentModerationArgs searchContentModerationArgs)
        {
            var requestArgs = searchContentModerationArgs ?? new SearchContentModerationArgs();
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_ModerationApiClient.SearchContentModerationAsync(
                new Generated.Moderation.SearchContentModerationRequest(ProjectId, EnvironmentId, requestArgs.Offset, requestArgs.Limit,
                    requestArgs.SortBys, requestArgs.Search, requestArgs.Filters, requestArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Content(x));
            return new PagedResults<Content>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<Subscription> CreateSubscriptionAsync(string contentId)
        {
            var result = await ExecuteAsyncWithReturn(() => m_SubscriptionApiClient.CreateSubscriptionAsync(new CreateSubscriptionRequest(
                new NewSubscriptionRequest(ProjectId, EnvironmentId, contentId)), Configuration));
            return new Subscription(result);
        }

        public async Task DeleteSubscriptionAsync(string contentId)
        {
            await ExecuteAsync(() => m_SubscriptionApiClient.DeleteSubscriptionAsync(new DeleteSubscriptionRequest(
                ProjectId, EnvironmentId, contentId), Configuration), 204);
        }

        public async Task<bool> IsSubscribedToAsync(string contentId)
        {
            try
            {
                await ExecuteAsync(() => m_SubscriptionApiClient.GetSubscriptionAsync(new GetSubscriptionRequest(
                    ProjectId, EnvironmentId, contentId), Configuration));
                return true;
            }
            catch (UgcException e)
            {
                if (e.ErrorCode == UgcErrorCodes.SubscriptionNotFound)
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<PagedResults<Subscription>> GetSubscriptionsAsync(GetSubscriptionsArgs getSubscriptionsArgs)
        {
            var requestArgs = getSubscriptionsArgs ?? new GetSubscriptionsArgs();
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_SubscriptionApiClient.SearchSubscriptionsAsync(
                new SearchSubscriptionsRequest(requestArgs.Offset, requestArgs.Limit,
                    requestArgs.SortBys, requestArgs.Search, requestArgs.Filters, requestArgs.IncludeTotal), Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Subscription(x));
            return new PagedResults<Subscription>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<Representation> CreateRepresentationAsync(
            CreateRepresentationArgs createRepresentationArgs)
        {
            var result = await ExecuteAsyncWithReturn(() => m_RepresentationApiClient.CreateRepresentationAsync(
                new CreateRepresentationRequest(ProjectId, EnvironmentId, createRepresentationArgs.ContentId,
                    new Generated.Models.CreateRepresentationRequest(createRepresentationArgs.Tags,
                        createRepresentationArgs.Metadata)), Configuration
            ));
            return new Representation(result);
        }

        public async Task<Representation> GetRepresentationAsync(GetRepresentationArgs getRepresentationArgs)
        {
            var result = await ExecuteAsyncWithReturn(
                () => m_RepresentationApiClient.GetRepresentationAsync(
                    new GetRepresentationRequest(ProjectId, EnvironmentId, getRepresentationArgs.ContentId,
                        getRepresentationArgs.RepresentationId), Configuration
                )
            );

            var representation = new Representation(result);
            if (getRepresentationArgs.DownloadRepresentation)
            {
                await DownloadRepresentationVersionDataAsync(representation);
            }

            return representation;
        }

        public async Task<Representation> UpdateRepresentationAsync(
            UpdateRepresentationArgs updateRepresentationArgs)
        {
            var result = await ExecuteAsyncWithReturn(() => m_RepresentationApiClient.UpdateRepresentationAsync(
                new UpdateRepresentationRequest(ProjectId, EnvironmentId, updateRepresentationArgs.ContentId,
                    updateRepresentationArgs.RepresentationId,
                    new Generated.Models.UpdateRepresentationRequest(updateRepresentationArgs.Tags,
                        updateRepresentationArgs.Version, updateRepresentationArgs.Metadata)), Configuration));
            return new Representation(result);
        }

        public async Task<RepresentationVersion> CreateRepresentationVersionAsync(string contentId,
            string representationId, Stream asset)
        {
            var maxRetryAttempts = Configuration.NumberOfRetries.GetValueOrDefault();
            var currentAttempt = 0;

            UgcValidators.ValidateStream(asset);
            var assetMd5Hash = GetHash(m_MD5, asset);

            do
            {
                try
                {
                    var uploadResponse = await ExecuteAsyncWithReturn(() =>
                        m_RepresentationApiClient.CreateRepresentationOutputVersionAsync(
                            new CreateRepresentationOutputVersionRequest(ProjectId, EnvironmentId, contentId,
                                representationId,
                                new AddRepresentationVersionRequest(assetMd5Hash)),
                            Configuration));

                    await SendUploadRepresentationOutputRequest(uploadResponse, asset);
                    return new RepresentationVersion(uploadResponse.RepresentationVersion);
                }
                catch (RequestFailedException e)
                {
                    if (e.ErrorCode != UgcErrorCodes.PreconditionFailed)
                    {
                        throw UgcException.Create(UgcErrorCodes.UploadContentFailed,
                            "Upload to GCP failed - could not send the content to GCP, see inner exception for details",
                            e);
                    }

                    currentAttempt++;
                }
            }
            while (currentAttempt < maxRetryAttempts);

            throw UgcException.Create(UgcErrorCodes.UploadContentFailed,
                "Upload to GCP failed - could not send the representation output to GCP, see inner exception for details");
        }

        public async Task<PagedResults<Representation>> GetRepresentationsAsync(
            GetRepresentationsArgs getRepresentationsArgs)
        {
            var dtoPagedResult = await ExecuteAsyncWithReturn(() => m_RepresentationApiClient.SearchRepresentationsAsync(
                new SearchRepresentationsRequest(ProjectId, EnvironmentId, getRepresentationsArgs.ContentId,
                    getRepresentationsArgs.Offset,
                    getRepresentationsArgs.Limit,
                    getRepresentationsArgs.Search,
                    getRepresentationsArgs.SortBys, getRepresentationsArgs.Tags, getRepresentationsArgs.Filters, getRepresentationsArgs.IncludeTotal),
                Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Representation(x));
            return new PagedResults<Representation>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<PagedResults<Representation>> SearchRepresentationsAsync(
            SearchRepresentationsArgs searchRepresentationsArgs)
        {
            var dtoPagedResult = await ExecuteAsyncWithReturn(() =>
                m_RepresentationApiClient.SearchProjectRepresentationsAsync(
                    new SearchProjectRepresentationsRequest(ProjectId, EnvironmentId,
                        searchRepresentationsArgs.Offset,
                        searchRepresentationsArgs.Limit,
                        searchRepresentationsArgs.SortBys,
                        searchRepresentationsArgs.Search,
                        searchRepresentationsArgs.Tags,
                        searchRepresentationsArgs.Filters,
                        searchRepresentationsArgs.IncludeTotal),
                    Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new Representation(x));
            return new PagedResults<Representation>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task<PagedResults<RepresentationVersion>> GetRepresentationVersionsAsync(
            GetRepresentationVersionsArgs getRepresentationVersionsArgs)
        {
            var dtoPagedResult = await ExecuteAsyncWithReturn(() =>
                m_RepresentationApiClient.SearchRepresentationVersionsAsync(
                    new SearchRepresentationVersionsRequest(ProjectId, EnvironmentId,
                        getRepresentationVersionsArgs.ContentId,
                        getRepresentationVersionsArgs.RepresentationId,
                        getRepresentationVersionsArgs.Offset,
                        getRepresentationVersionsArgs.Limit,
                        getRepresentationVersionsArgs.SortBys,
                        getRepresentationVersionsArgs.IncludeTotal),
                    Configuration));
            var results = dtoPagedResult.Results.ConvertAll(x => new RepresentationVersion(x));
            return new PagedResults<RepresentationVersion>(dtoPagedResult.Offset, dtoPagedResult.Limit, dtoPagedResult.Total, results);
        }

        public async Task DeleteRepresentationAsync(DeleteRepresentationArgs deleteRepresentation)
        {
            await ExecuteAsync(() => m_RepresentationApiClient.DeleteRepresentationAsync(
                new DeleteRepresentationRequest(ProjectId, EnvironmentId, deleteRepresentation.ContentId, deleteRepresentation.RepresentationId), Configuration), 204);
        }

        public async Task DownloadRepresentationVersionDataAsync(Representation representation)
        {
            try
            {
                var downloadContentTask = DownloadRequest(representation.DownloadUrl);
                await downloadContentTask;
                if (downloadContentTask.Result != null)
                {
                    representation.DownloadedContent = downloadContentTask.Result;
                }
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == CommonErrorCodes.NotFound)
            {
                throw UgcException.Create(UgcErrorCodes.RepresentationNotFound,
                    $"The requested representation version {representation.Id} data cannot be found.", ex);
            }
        }

        public async Task DownloadContentDataAsync(Content content, bool downloadContent, bool downloadThumbnail)
        {
            var downloadContentTask = m_CompletedDownload;
            var downloadThumbnailTask = m_CompletedDownload;

            try
            {
                if (downloadContent)
                {
                    downloadContentTask = DownloadRequest(content.DownloadUrl);
                }

                if (downloadThumbnail && !string.IsNullOrEmpty(content.ThumbnailUrl))
                {
                    downloadThumbnailTask = DownloadRequest(content.ThumbnailUrl);
                }

                await Task.WhenAll(downloadContentTask, downloadThumbnailTask);
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == CommonErrorCodes.NotFound)
            {
                if (downloadContentTask.IsFaulted)
                {
                    throw UgcException.Create(UgcErrorCodes.ContentNotFound,
                        $"The requested content {content.Id} data cannot be found.", ex);
                }

                throw UgcException.Create(UgcErrorCodes.ContentNotFound,
                    $"The requested content {content.Id} thumbnail cannot be found.", ex);
            }

            if (downloadContent && downloadContentTask.Result != null)
            {
                content.DownloadedContent = downloadContentTask.Result;
            }

            if (downloadThumbnail && downloadThumbnailTask.Result != null)
            {
                content.DownloadedThumbnail = downloadThumbnailTask.Result;
            }
        }

        internal async Task<T> ExecuteAsync<T>(Func<Task<T>> asyncAction, long statusCode = 200) where T : Response
        {
            ValidateRequiredDependencies();

            try
            {
                var res = await asyncAction();

                res.EnsureUgcSuccessStatusCode(statusCode);

                return res;
            }
            catch (HttpException<ApiErrorInfo> e)
            {
                throw UgcException.Create(e.ActualError.Status, $"{e.ActualError.Title} {e.ActualError.Detail}", e);
            }
            catch (HttpException e)
            {
                UgcValidators.CheckForClientServerErrorCode(e.Response.StatusCode);
                throw;
            }
            catch (ResponseDeserializationException e)
            {
                throw UgcException.HandleDeserializationException(e);
            }
            catch (Exception e)
            {
                if (e is RequestFailedException)
                    throw;
                throw UgcException.HandleUnknownException(e);
            }
        }

        internal async Task<T> ExecuteAsyncWithReturn<T>(Func<Task<Response<T>>> asyncAction, long statusCode = 200)
        {
            var res = await ExecuteAsync(asyncAction, statusCode);
            UgcValidators.ValidateResult(res);
            return res.Result;
        }

        internal void ValidateRequiredDependencies()
        {
            if (string.IsNullOrEmpty(m_CloudProjectIdComponent.GetCloudProjectId()))
                throw UgcException.Create(UgcErrorCodes.Unauthorized, "Project ID is missing - make sure the project is correctly linked to your game and try again.");
            if (string.IsNullOrEmpty(m_AccessTokenComponent.AccessToken))
                throw UgcException.Create(UgcErrorCodes.Unauthorized, "Access token is missing - ensure you are signed in through the Authentication SDK and try again.");
        }

        internal async Task<HttpResponseMessage> SendUploadRepresentationOutputRequest(
            UploadRepresentationVersionResponse uploadResponse, Stream asset)
        {
            var requestTimeout = Configuration.RequestTimeout.GetValueOrDefault();
            var uploadAssetResult = await m_ContentHttpClient.SendPutRequestAsync(uploadResponse.UploadUrl, asset,
                uploadResponse.UploadHeaders, requestTimeout);

            UgcValidators.CheckForClientServerErrorCode((long)uploadAssetResult.StatusCode);
            return uploadAssetResult;
        }

        internal async Task<HttpResponseMessage[]> SendUploadContentRequest(UploadContentResponse uploadContentResponse, Stream asset, Stream thumbnail)
        {
            var requestTimeout = Configuration.RequestTimeout.GetValueOrDefault();
            var uploadAssetTask = m_ContentHttpClient.SendPutRequestAsync(uploadContentResponse.UploadContentUrl, asset,
                uploadContentResponse.UploadContentHeaders, requestTimeout);
            Task<HttpResponseMessage> uploadThumbnailTask = null;
            var tasks = new List<Task<HttpResponseMessage>>() { uploadAssetTask };

            if (thumbnail != null)
            {
                uploadThumbnailTask = m_ContentHttpClient.SendPutRequestAsync(uploadContentResponse.UploadThumbnailUrl,
                    thumbnail, uploadContentResponse.UploadThumbnailHeaders, requestTimeout);
                tasks.Add(uploadThumbnailTask);
            }

            var result = await Task.WhenAll(tasks);

            UgcValidators.CheckForClientServerErrorCode((long)uploadAssetTask.Result.StatusCode);
            if (uploadThumbnailTask != null)
            {
                UgcValidators.CheckForClientServerErrorCode((long)uploadThumbnailTask.Result.StatusCode);
            }

            return result;
        }

        async Task<byte[]> DownloadRequest(string url)
        {
            var requestTimeout = Configuration.RequestTimeout.GetValueOrDefault();
            var downloadTask = await m_ContentHttpClient.SendGetRequestAsync(url, requestTimeout);
            UgcValidators.CheckForClientServerErrorCode((long)downloadTask.StatusCode);
            return await downloadTask.Content.ReadAsByteArrayAsync();
        }

        static string GetHash(HashAlgorithm hashAlgorithm, Stream input)
        {
            input.Position = 0;
            var data = hashAlgorithm.ComputeHash(input);
            return Convert.ToBase64String(data);
        }
    }

    class ContentHttpClient : IContentHttpClient
    {
        readonly HttpClient m_HttpClient;

        internal ContentHttpClient()
        {
            m_HttpClient = new HttpClient();
        }

        internal ContentHttpClient(HttpClient client)
        {
            m_HttpClient = client;
        }

        public async Task<HttpResponseMessage> SendGetRequestAsync(string url, int requestTimeout,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using (var requestCts = new CancellationTokenSource(new TimeSpan(0, 0, requestTimeout)))
                {
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(requestCts.Token, cancellationToken))
                    {
                        return await m_HttpClient.GetAsync(url, linkedCts.Token);
                    }
                }
            }
            catch (TaskCanceledException e)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw UgcException.Create(UgcErrorCodes.UploadContentCanceled, "Download failed - the task was canceled by user.", e);
                }

                throw UgcException.Create(UgcErrorCodes.UploadContentCanceled, "Download failed - the task was canceled due to timeout.", e);
            }
        }

        public async Task<HttpResponseMessage> SendPutRequestAsync(string url, Stream bodyStream,
            IDictionary<string, List<string>> uploadContentHeaders, int requestTimeout, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var requestCts = new CancellationTokenSource(new TimeSpan(0, 0, requestTimeout)))
                {
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(requestCts.Token, cancellationToken))
                    {
                        bodyStream.Position = 0;
                        var httpContent = new StreamContent(bodyStream);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        foreach (var header in uploadContentHeaders)
                        {
                            httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                        }

                        return await m_HttpClient.PutAsync(url, httpContent, linkedCts.Token);
                    }
                }
            }
            catch (TaskCanceledException e)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw UgcException.Create(UgcErrorCodes.UploadContentCanceled, "Upload failed - the task was canceled by user.", e);
                }

                throw UgcException.Create(UgcErrorCodes.UploadContentCanceled, "Upload failed - the task was canceled due to timeout.", e);
            }
        }
    }
}
