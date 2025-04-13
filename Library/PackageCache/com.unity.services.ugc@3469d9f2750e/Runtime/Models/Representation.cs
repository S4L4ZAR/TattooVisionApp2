using System;
using System.Collections.Generic;
using Unity.Services.Ugc.Generated.Models;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// Contains source content representation metadata
    /// </summary>
    public class Representation
    {
        internal Representation(RepresentationDTO representationDto)
        {
            Id = representationDto.Id;
            ContentId = representationDto.ContentId;
            CreatedAt = representationDto.CreatedAt;
            UpdatedAt = representationDto.UpdatedAt;
            CurrentVersion = representationDto.CurrentVersion;
            Tags = representationDto.Tags?.ConvertAll(x => new RepresentationTag(x));
            DownloadUrl = representationDto.DownloadUrl;
            Metadata = representationDto.Metadata;
        }

        /// <summary>
        /// Id of representation
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Id of source content the representation applies to
        /// </summary>
        public string ContentId { get; }

        /// <summary>
        /// Representation tags.
        /// </summary>
        public List<RepresentationTag> Tags { get; }

        /// <summary>
        /// Date rating was first created
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Date rating was updated
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <summary>
        /// Current version of representation
        /// </summary>
        public string CurrentVersion { get; }

        /// <summary>
        /// Metadata of the representation
        /// </summary>
        public string Metadata { get; }

        /// <summary>
        /// The downloaded represenation version. This value is only set with `GetRepresentationAsync` or `DownloadRepresentationVersionDataAsync`
        /// </summary>
        public byte[] DownloadedContent { get; internal set; }

        internal string DownloadUrl { get; }
    }
}
