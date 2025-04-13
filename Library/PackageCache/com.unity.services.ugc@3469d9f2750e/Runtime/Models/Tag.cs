using Unity.Services.Ugc.Generated.Models;

namespace Unity.Services.Ugc
{
    /// <summary>
    /// Contains tag metadata
    /// </summary>
    public class Tag
    {
        internal Tag(TagDTO tagDTO)
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
