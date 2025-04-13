using Unity.Services.Ugc.Generated.Models;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// Contains representation tag metadata
    /// </summary>
    public class RepresentationTag
    {
        internal RepresentationTag(RepresentationTagDTO tagDTO)
        {
            Id = tagDTO.Id;
            Name = tagDTO.Name;
        }

        /// <summary>
        /// Tag item guid
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; }
    }
}
