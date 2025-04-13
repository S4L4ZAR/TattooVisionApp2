using System;
using System.Threading.Tasks;
using Unity.Services.Authentication.Internal;
using Unity.Services.Core.Configuration.Internal;
using Unity.Services.Core.Internal;
using Unity.Services.Ugc.Generated;
using Unity.Services.Ugc.Generated.Apis.Content;
using Unity.Services.Ugc.Generated.Apis.ContentVersions;
using Unity.Services.Ugc.Generated.Apis.Moderation;
using Unity.Services.Ugc.Generated.Apis.Representation;
using Unity.Services.Ugc.Generated.Apis.Subscription;
using Unity.Services.Ugc.Generated.Apis.Tag;
using Unity.Services.Ugc.Generated.Http;
using Unity.Services.Ugc.Internal;
using UnityEngine;

namespace Unity.Services.Ugc
{
    class UgcInternalServiceProvider : IInitializablePackage
    {
        public Task Initialize(CoreRegistry registry)
        {
            var httpClient = new HttpClient();

            var accessTokenComponent = registry.GetServiceComponent<IAccessToken>();
            var cloudProjectIdComponent = registry.GetServiceComponent<ICloudProjectId>();
            var environmentIdComponent = registry.GetServiceComponent<IEnvironmentId>();
            var projectConfiguration = registry.GetServiceComponent<IProjectConfiguration>();

            var contentApi = new ContentApiClient(httpClient, accessTokenComponent);
            var contentVersionsApi = new ContentVersionsApiClient(httpClient, accessTokenComponent);
            var subscriptionApi = new SubscriptionApiClient(httpClient, accessTokenComponent);
            var tagApi = new TagApiClient(httpClient, accessTokenComponent);
            var representationApi = new RepresentationApiClient(httpClient, accessTokenComponent);
            var moderationApi = new ModerationApiClient(httpClient, accessTokenComponent);

            var basePath = UgcServiceSdkUtils.GetHost(projectConfiguration);
            var configuration = new Configuration(basePath, 100, 4, null);

            var wrappedUgcService = new WrappedUgcService(contentApi, tagApi, subscriptionApi, contentVersionsApi,
                representationApi, moderationApi, new ContentHttpClient(),
                accessTokenComponent, cloudProjectIdComponent, environmentIdComponent, configuration);
            UgcInternalService.Instance = wrappedUgcService;
            UgcService.Instance = wrappedUgcService;

            return Task.CompletedTask;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            // Pass an instance of this class to Core
            var generatedPackageRegistry =
                CoreRegistry.Instance.RegisterPackage(new UgcInternalServiceProvider());

            // And specify what components it requires, or provides.
            generatedPackageRegistry.DependsOn<IAccessToken>();
            generatedPackageRegistry.DependsOn<ICloudProjectId>();
            generatedPackageRegistry.DependsOn<IEnvironmentId>();
            generatedPackageRegistry.DependsOn<IProjectConfiguration>();
        }
    }
}
