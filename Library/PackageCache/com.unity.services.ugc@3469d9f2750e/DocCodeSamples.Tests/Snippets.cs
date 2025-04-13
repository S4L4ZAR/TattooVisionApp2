using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Ugc;
using Unity.Services.Ugc.Generated.Models;
using UnityEngine;

public class Snippets : MonoBehaviour
{
    #region convert-content-thumbnail-to-sprite
    public Sprite ConvertContentThumbnailToSprite(Content content)
    {
        var downloadedTexture = new Texture2D(2, 2);
        downloadedTexture.LoadImage(content.DownloadedThumbnail);

        var sprite = Sprite.Create(
            texture: downloadedTexture,
            rect: new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height),
            pivot: new Vector2(0.5f, 0.5f),
            pixelsPerUnit: 100.0f);

        return sprite;
    }

    #endregion

    #region create-content-async
    public async Task<Content> CreateNewContentAsync()
    {
        using var contentStream = File.Open("<path-to-content-file>", FileMode.Open, FileAccess.Read, FileShare.Read);
        using var thumbnailStream = File.Open("<path-to-the-image-representation-of-the-content>", FileMode.Open, FileAccess.Read, FileShare.Read);

        var content = await UgcService.Instance.CreateContentAsync(new CreateContentArgs("A new content", "A description", contentStream) { IsPublic = true, TagsId = null, Thumbnail = thumbnailStream});

        Debug.Log($"Created content with id: {content.Id}.");

        return content;
    }

    #endregion

    #region create-representation-async
    public async Task<Representation> CreateNewRepresentationAsync(Content content)
    {
        var tags = new List<string>() { "example" };
        var representation = await UgcService.Instance.CreateRepresentationAsync(new CreateRepresentationArgs(content.Id, tags));

        Debug.Log($"Created representation with id: {representation.Id}.");

        return representation;
    }

    #endregion

    #region create-representation-version-async
    public async Task<RepresentationVersion> CreateNewRepresentationVersionAsync(Content content, Representation representation)
    {
        using var contentStream = File.Open("<path-to-content-file>", FileMode.Open, FileAccess.Read, FileShare.Read);
        var representationVersion = await UgcService.Instance.CreateRepresentationVersionAsync(content.Id, representation.Id, contentStream);

        Debug.Log($"Created representation version with id: {representationVersion.Id}.");

        return representationVersion;
    }

    #endregion

    #region delete-content-async
    public async void DeleteContentAsync(string contentId)
    {
        await UgcService.Instance.DeleteContentAsync(contentId);

        Debug.Log($"Deleted content with id: {contentId}.");
    }

    #endregion

    #region delete-representation-async
    public async void DeleteRepresentationAsync(string contentId, string representationId)
    {
        await UgcService.Instance.DeleteRepresentationAsync(new DeleteRepresentationArgs(contentId, representationId));

        Debug.Log($"Deleted representation with id: {representationId}.");
    }

    #endregion

    #region get-content-async
    public async Task<Content> GetContentByIdAsync(string contentId)
    {
        var content = await UgcService.Instance.GetContentAsync(new GetContentArgs(contentId) { DownloadContent = true, DownloadThumbnail = true});

        Debug.Log($"Retrieved content with id: {content.Id}.");

        return content;
    }

    #endregion

    #region get-content-by-name-async
    public async Task<string> GetContentIdAsync(string name)
    {
        var contents = await UgcService.Instance.GetContentsAsync(new GetContentsArgs());
        var contentId = "";

        foreach (var content in contents.Results)
        {
            if (content.Name == name)
            {
                contentId = content.Id;
                break;
            }
        }

        if (!string.IsNullOrEmpty(contentId))
        {
            Debug.Log($"Content id retrieved for {name}: {contentId}");
        }

        return contentId;
    }

    #endregion

    #region get-contents-async
    public async Task<List<Content>> GetContentsListAsync()
    {
        var contents = await UgcService.Instance.GetContentsAsync(new GetContentsArgs());

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-player-contents-async
    public async Task<List<Content>> GetPlayerContentAsync()
    {
        var contents = await UgcService.Instance.GetPlayerContentsAsync(new GetPlayerContentsArgs());

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-content-trends-async

    public async Task<List<Content>> GetContentTrendsAsync()
    {
        var contents = await UgcService.Instance.GetContentTrendsAsync(new GetContentTrendsArgs(ContentTrendType.TopRated));
        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-contents-page-async
    public async Task<List<Content>> GetContentsListByPageAsync(int pageIndex, int contentPerPage)
    {
        var offset = pageIndex * contentPerPage;
        var limit = (pageIndex + 1) * contentPerPage;

        var getContentsArgs = new GetContentsArgs()
        {
            Offset = offset,
            Limit = limit
        };

        var contents = await UgcService.Instance.GetContentsAsync(getContentsArgs);

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-contents-search-async
    public async Task<List<Content>> GetContentsListBySearchAsync(string searchFilter)
    {
        var getContentsArgs = new GetContentsArgs()
        {
            Search = searchFilter
        };

        var contents = await UgcService.Instance.GetContentsAsync(getContentsArgs);

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-content-with-reporttype-async
    public async Task<List<Content>> GetContentsListBySearchWithReportAsync(string searchFilter)
    {
        var getContentsArgs = new GetContentsArgs()
        {
            Search = searchFilter
        };

        var contents = await UgcService.Instance.GetContentsAsync(getContentsArgs);

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion

    #region get-preview-thumbnail
    public void GetPreviewThumbnail()
    {
        // the process can take few seconds to write the image, refresh your editor if you do not see it.
        ScreenCapture.CaptureScreenshot("Assets/Thumbnail.png");
    }

    #endregion

    #region get-representation-async
    public async Task<Representation> GetRepresentationByIdAsync(string contentId, string representationId)
    {
        var representation = await UgcService.Instance.GetRepresentationAsync(new GetRepresentationArgs(contentId, representationId) { DownloadRepresentation = true });

        Debug.Log($"Retrieved representation with id: {representation.Id}.");

        return representation;
    }

    #endregion

    #region get-representations-async
    public async Task<List<Representation>> GetRepresentationsListAsync(string contentId)
    {
        var representations = await UgcService.Instance.GetRepresentationsAsync(new GetRepresentationsArgs(contentId));

        Debug.Log("The following representations were retrieved:");

        foreach (var representation in representations.Results)
        {
            Debug.Log(representation.Id);
        }

        return representations.Results;
    }

    #endregion

    #region search-representations-in-project-async
    public async Task<List<Representation>> SearchRepresentationsInProjectAsync(string searchFilter)
    {
        var searchRepresentationsArgs = new SearchRepresentationsArgs(searchFilter);

        var representations = await UgcService.Instance.SearchRepresentationsAsync(searchRepresentationsArgs);

        Debug.Log("The following representations were retrieved:");

        foreach (var representation in representations.Results)
        {
            Debug.Log(representation.Id);
        }

        return representations.Results;
    }

    #endregion

    #region get-representation-versions-async
    public async Task<List<RepresentationVersion>> GetRepresentationVersionsListAsync(string contentId, string representationId)
    {
        var representationVersions = await UgcService.Instance.GetRepresentationVersionsAsync(new GetRepresentationVersionsArgs(contentId, representationId));

        Debug.Log("The following representation versions were retrieved:");

        foreach (var representationVersion in representationVersions.Results)
        {
            Debug.Log(representationVersion.Id);
        }

        return representationVersions.Results;
    }

    #endregion

    #region get-subscriptions-async
    public async Task<List<Subscription>> GetSubscribedContentListAsync()
    {
        var subscriptions = await UgcService.Instance.GetSubscriptionsAsync(new GetSubscriptionsArgs());

        Debug.Log("Player is subscribed to the following contents:");

        foreach (var subscription in subscriptions.Results)
        {
            Debug.Log(subscription.Content.Id);
        }

        return subscriptions.Results;
    }

    #endregion

    #region get-tags-async
    public async Task<List<Tag>> GetTagsListAsync()
    {
        var tags = await UgcService.Instance.GetTagsAsync();
        Debug.Log("The following tags were retrieved:");

        foreach (var tag in tags)
        {
            Debug.Log(tag);
        }

        return tags;
    }

    #endregion

    #region sign-in-anon-async
    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    #endregion

    #region sign-in-apple-async
    public async Task SignInWithAppleAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithAppleAsync(idToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    #endregion

    #region rate-content-async
    public async void SubmitUserContentRatingAsync(string contentId, float rating)
    {
        await UgcService.Instance.SubmitUserContentRatingAsync(contentId, rating);

        Debug.Log($"The following content was rated: {contentId} {rating}/5.");
    }

    #endregion

    #region subscribe-content-async
    public async void SubscribeToContentAsync(string contentId)
    {
        await UgcService.Instance.CreateSubscriptionAsync(contentId);

        Debug.Log($"Subscribed to the following content: {contentId}.");
    }

    #endregion

    #region unsubscribe-content-async
    public async void UnsubscribeFromContentAsync(string contentId)
    {
        await UgcService.Instance.DeleteSubscriptionAsync(contentId);

        Debug.Log($"Unsubscribed from the following content: {contentId}.");
    }

    #endregion

    #region create-content-version-async
    public async Task<Content> CreateContentVersionAsync(string contentId)
    {
        using var contentStream = File.Open("<path-to-content-file>", FileMode.Open, FileAccess.Read, FileShare.Read);
        using var thumbnailStream = File.Open("<path-to-the-image-representation-of-the-content>", FileMode.Open, FileAccess.Read, FileShare.Read);

        var content = await UgcService.Instance.CreateContentVersionAsync(contentId, contentStream, thumbnailStream);

        Debug.Log($"Updated the following content: {content.Id} version: {content.Version}.");

        return content;
    }

    #endregion

    #region update-content-details-async
    public async Task<Content> UpdateContentDetailsAsync(string contentId)
    {
        var metadata = new UpdateContentDetailsArgs(contentId, "a new title", "A another new description!", true, new List<string>() { "Art" });
        var content = await UgcService.Instance.UpdateContentDetailsAsync(metadata);

        Debug.Log($"Updated the following content details: {content.Id} version: {content.Version}.");

        return content;
    }

    #endregion

    #region update-representation-async
    public async Task<Representation> UpdateRepresentationAsync(Content content, Representation representation)
    {
        using var contentStream = File.Open("<path-to-content-file>", FileMode.Open, FileAccess.Read, FileShare.Read);
        var representationVersion = await UgcService.Instance.CreateRepresentationVersionAsync(content.Id, representation.Id, contentStream);

        Debug.Log($"Created representation version with id: {representationVersion.Id}.");

        var tags = new List<string>() { "example" };
        var newRepresentation = await UgcService.Instance.UpdateRepresentationAsync(new UpdateRepresentationArgs(content.Id, representation.Id, tags, representationVersion.Id));

        Debug.Log($"Updated representation version with id: {newRepresentation.Id}.");

        return newRepresentation;
    }

    #endregion

    #region report-content-async
    public async void ReportContentAsync(string contentId)
    {
        var result = await UgcService.Instance.ReportContentAsync(new ReportContentArgs(contentId, Reason.NonFunctional));

        Debug.Log($"The following content was reported: {result.Id}.");
    }

    #endregion

    #region approve-content-async
    public async void ApproveContentAsync(string contentId)
    {
        var result = await UgcService.Instance.ApproveContentAsync(contentId);

        Debug.Log($"The following content was approved: {result.Id}.");
    }

    #endregion

    #region reject-content-async
    public async void RejectContentAsync(string contentId)
    {
        var result = await UgcService.Instance.RejectContentAsync(contentId);

        Debug.Log($"The following content was rejected: {result.Id}.");
    }

    #endregion

    #region search-content-moderation-async
    public async Task<List<Content>> SearchContentModerationAsync()
    {
        var contents = await UgcService.Instance.SearchContentModerationAsync(new SearchContentModerationArgs());

        Debug.Log("The following contents were retrieved:");

        foreach (var content in contents.Results)
        {
            Debug.Log(content.Id);
        }

        return contents.Results;
    }

    #endregion
}
