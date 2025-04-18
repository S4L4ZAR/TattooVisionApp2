//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.Ugc.Generated.Http;



namespace Unity.Services.Ugc.Generated.Models
{
    /// <summary>
    /// ModeratorDTO model
    /// </summary>
    [Preserve]
    [DataContract(Name = "ModeratorDTO")]
    internal class ModeratorDTO
    {
        /// <summary>
        /// Creates an instance of ModeratorDTO.
        /// </summary>
        /// <param name="playerId">playerId param</param>
        /// <param name="environmentId">environmentId param</param>
        /// <param name="projectId">projectId param</param>
        /// <param name="playerRoleId">playerRoleId param</param>
        /// <param name="createdAt">createdAt param</param>
        /// <param name="updatedAt">updatedAt param</param>
        /// <param name="environment">environment param</param>
        /// <param name="playerRole">playerRole param</param>
        /// <param name="bannedPlayers">bannedPlayers param</param>
        /// <param name="contentModerationAuditLogs">contentModerationAuditLogs param</param>
        /// <param name="statistics">statistics param</param>
        /// <param name="deletedAt">deletedAt param</param>
        [Preserve]
        public ModeratorDTO(string playerId, string environmentId, string projectId, System.Guid playerRoleId, DateTime createdAt, DateTime updatedAt, EnvironmentDTO environment = default, PlayerRoleDTO playerRole = default, List<BannedPlayerDTO> bannedPlayers = default, List<ContentModerationAuditLogDTO> contentModerationAuditLogs = default, ModeratorStatisticsDTO statistics = default, DateTime? deletedAt = default)
        {
            PlayerId = playerId;
            EnvironmentId = environmentId;
            ProjectId = projectId;
            PlayerRoleId = playerRoleId;
            Environment = environment;
            PlayerRole = playerRole;
            BannedPlayers = bannedPlayers;
            ContentModerationAuditLogs = contentModerationAuditLogs;
            Statistics = statistics;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeletedAt = deletedAt;
        }

        /// <summary>
        /// Parameter playerId of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "playerId", IsRequired = true, EmitDefaultValue = true)]
        public string PlayerId{ get; }
        
        /// <summary>
        /// Parameter environmentId of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "environmentId", IsRequired = true, EmitDefaultValue = true)]
        public string EnvironmentId{ get; }
        
        /// <summary>
        /// Parameter projectId of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "projectId", IsRequired = true, EmitDefaultValue = true)]
        public string ProjectId{ get; }
        
        /// <summary>
        /// Parameter playerRoleId of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "playerRoleId", IsRequired = true, EmitDefaultValue = true)]
        public System.Guid PlayerRoleId{ get; }
        
        /// <summary>
        /// Parameter environment of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "environment", EmitDefaultValue = false)]
        public EnvironmentDTO Environment{ get; }
        
        /// <summary>
        /// Parameter playerRole of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "playerRole", EmitDefaultValue = false)]
        public PlayerRoleDTO PlayerRole{ get; }
        
        /// <summary>
        /// Parameter bannedPlayers of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "bannedPlayers", EmitDefaultValue = false)]
        public List<BannedPlayerDTO> BannedPlayers{ get; }
        
        /// <summary>
        /// Parameter contentModerationAuditLogs of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "contentModerationAuditLogs", EmitDefaultValue = false)]
        public List<ContentModerationAuditLogDTO> ContentModerationAuditLogs{ get; }
        
        /// <summary>
        /// Parameter statistics of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "statistics", EmitDefaultValue = false)]
        public ModeratorStatisticsDTO Statistics{ get; }
        
        /// <summary>
        /// Parameter createdAt of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "createdAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime CreatedAt{ get; }
        
        /// <summary>
        /// Parameter updatedAt of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "updatedAt", IsRequired = true, EmitDefaultValue = true)]
        public DateTime UpdatedAt{ get; }
        
        /// <summary>
        /// Parameter deletedAt of ModeratorDTO
        /// </summary>
        [Preserve]
        [DataMember(Name = "deletedAt", EmitDefaultValue = false)]
        public DateTime? DeletedAt{ get; }
    
        /// <summary>
        /// Formats a ModeratorDTO into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            if (PlayerId != null)
            {
                serializedModel += "playerId," + PlayerId + ",";
            }
            if (EnvironmentId != null)
            {
                serializedModel += "environmentId," + EnvironmentId + ",";
            }
            if (ProjectId != null)
            {
                serializedModel += "projectId," + ProjectId + ",";
            }
            if (PlayerRoleId != null)
            {
                serializedModel += "playerRoleId," + PlayerRoleId + ",";
            }
            if (Environment != null)
            {
                serializedModel += "environment," + Environment.ToString() + ",";
            }
            if (PlayerRole != null)
            {
                serializedModel += "playerRole," + PlayerRole.ToString() + ",";
            }
            if (BannedPlayers != null)
            {
                serializedModel += "bannedPlayers," + BannedPlayers.ToString() + ",";
            }
            if (ContentModerationAuditLogs != null)
            {
                serializedModel += "contentModerationAuditLogs," + ContentModerationAuditLogs.ToString() + ",";
            }
            if (Statistics != null)
            {
                serializedModel += "statistics," + Statistics.ToString() + ",";
            }
            if (CreatedAt != null)
            {
                serializedModel += "createdAt," + CreatedAt.ToString() + ",";
            }
            if (UpdatedAt != null)
            {
                serializedModel += "updatedAt," + UpdatedAt.ToString() + ",";
            }
            if (DeletedAt != null)
            {
                serializedModel += "deletedAt," + DeletedAt.ToString();
            }
            return serializedModel;
        }

        /// <summary>
        /// Returns a ModeratorDTO as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            if (PlayerId != null)
            {
                var playerIdStringValue = PlayerId.ToString();
                dictionary.Add("playerId", playerIdStringValue);
            }
            
            if (EnvironmentId != null)
            {
                var environmentIdStringValue = EnvironmentId.ToString();
                dictionary.Add("environmentId", environmentIdStringValue);
            }
            
            if (ProjectId != null)
            {
                var projectIdStringValue = ProjectId.ToString();
                dictionary.Add("projectId", projectIdStringValue);
            }
            
            if (PlayerRoleId != null)
            {
                var playerRoleIdStringValue = PlayerRoleId.ToString();
                dictionary.Add("playerRoleId", playerRoleIdStringValue);
            }
            
            if (CreatedAt != null)
            {
                var createdAtStringValue = CreatedAt.ToString();
                dictionary.Add("createdAt", createdAtStringValue);
            }
            
            if (UpdatedAt != null)
            {
                var updatedAtStringValue = UpdatedAt.ToString();
                dictionary.Add("updatedAt", updatedAtStringValue);
            }
            
            if (DeletedAt != null)
            {
                var deletedAtStringValue = DeletedAt.ToString();
                dictionary.Add("deletedAt", deletedAtStringValue);
            }
            
            return dictionary;
        }
    }
}
