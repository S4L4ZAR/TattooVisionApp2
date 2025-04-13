using System;
using Unity.Services.Ugc.Generated.Models;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// Contains subscription metadata
    /// </summary>
    public class Subscription
    {
        internal Subscription(SubscriptionDTO subscriptionDTO)
        {
            Id = subscriptionDTO.Id;
            PlayerId = subscriptionDTO.PlayerId;
            Content = new Content(subscriptionDTO.Content);
            CreatedAt = subscriptionDTO.CreatedAt;
            UpdatedAt = subscriptionDTO.UpdatedAt;
            DeletedAt = subscriptionDTO.DeletedAt;
        }

        /// <summary>
        /// Id of the subscription
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Id of the player
        /// </summary>
        public string PlayerId { get; }

        /// <summary>
        /// Subscribed content
        /// </summary>
        public Content Content { get; }

        /// <summary>
        /// Date subscription was created
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Date subscription was last updated
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <summary>
        /// Date subscription was soft deleted
        /// </summary>
        public DateTime? DeletedAt { get; }
    }
}
