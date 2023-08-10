using PlayFab;
using PlayFab.GroupsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayfabGuilds : MonoBehaviour
{
    public Action<string> OnGuildFeedback;
    public Action<List<string>> OnGuildListUpdated;
    public Action<List<string>> OnApplicationListUpdated;
    public Action<List<string>> OnMemberListUpdated;

    public readonly HashSet<KeyValuePair<string, string>> EntityGroupPairs = new HashSet<KeyValuePair<string, string>>();
    public readonly Dictionary<string, string> GroupNameById = new Dictionary<string, string>();

    public static EntityKey EntityKeyMaker(string entityId)
    {
        return new EntityKey { Id = entityId, Type = "title_player_account" };
    }

    private void OnSharedError(PlayFabError error)
    {
        string feedback = "Error: " + error.GenerateErrorReport();
        Debug.LogError(feedback);

        OnGuildFeedback?.Invoke(feedback);
    }

    public void ListGroups(string entityId)
    {
        Debug.Log("Entity ID: " + entityId); 
        if (string.IsNullOrEmpty(entityId))
        {
            Debug.LogError("Entity ID is missing!");
            return;
        }

        var request = new ListMembershipRequest { Entity = EntityKeyMaker(entityId) };
        PlayFabGroupsAPI.ListMembership(request, OnListGroups, OnSharedError);
    }

    private void OnListGroups(ListMembershipResponse response)
    {
        List<string> guildNames = new List<string>();

        foreach (var pair in response.Groups)
        {
            guildNames.Add(pair.GroupName);
            GroupNameById[pair.Group.Id] = pair.GroupName;
        }

        OnGuildListUpdated?.Invoke(guildNames);
    }

    public void LeaveGuildById(string groupId, string entityId)
    {
        if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(entityId))
        {
            OnGuildFeedback?.Invoke("Guild ID or Entity ID is missing!");
            return;
        }

        var request = new RemoveMembersRequest { Group = EntityKeyMaker(groupId), Members = new List<EntityKey> { EntityKeyMaker(entityId) } };
        PlayFabGroupsAPI.RemoveMembers(request, OnRemoveMember, OnSharedError);
    }

    private void OnRemoveMember(EmptyResponse response)
    {
        OnGuildFeedback?.Invoke("You have left the guild!");
    }

    public void CreateGuild(string guildName, string entityId)
    {
        var request = new CreateGroupRequest { GroupName = guildName, Entity = EntityKeyMaker(entityId) };
        PlayFabGroupsAPI.CreateGroup(request, OnCreateGroup, OnSharedError);
    }

    private void OnCreateGroup(CreateGroupResponse response)
    {
        string feedback = "Group Created: " + response.GroupName + " - " + response.Group.Id;
        Debug.Log(feedback);

        OnGuildFeedback?.Invoke(feedback);

        var prevRequest = (CreateGroupRequest)response.Request;
        EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, response.Group.Id));
        GroupNameById[response.Group.Id] = response.GroupName;
    }

    public void JoinGuildById(string guildId, string entityId)
    {
        if (string.IsNullOrEmpty(guildId))
        {
            OnGuildFeedback?.Invoke("Guild ID is missing!");
            return;
        }

        if (EntityGroupPairs.Any(pair => pair.Key == entityId && pair.Value == guildId))
        {
            OnGuildFeedback?.Invoke("You are already a member of this guild.");
            return;
        }

        var request = new ApplyToGroupRequest { Group = EntityKeyMaker(guildId), Entity = EntityKeyMaker(entityId) };
        PlayFabGroupsAPI.ApplyToGroup(request, OnApplyToGroup, OnSharedError);
    }


    private void OnApplyToGroup(ApplyToGroupResponse response)
    {
        var prevRequest = (ApplyToGroupRequest)response.Request;
        Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
        EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, prevRequest.Group.Id));
        OnGuildFeedback?.Invoke("You have joined the guild!");
    }

    public void ListGroupMembers(string groupId)
    {
        PlayFabGroupsAPI.ListGroupMembers(new ListGroupMembersRequest
        {
            Group = EntityKeyMaker(groupId)
        },
        OnListGroupMembers,
        OnSharedError);
    }

    private void OnListGroupMembers(ListGroupMembersResponse response)
    {
        List<string> memberNames = new List<string>();

        foreach (var entityMemberRole in response.Members)
        {
            foreach (var member in entityMemberRole.Members)
            {
                memberNames.Add(member.Key.Id);
            }
        }

        OnMemberListUpdated?.Invoke(memberNames);
    }

    public void AcceptAllApplications(string guildName)
    {
        string groupId = GroupNameById.FirstOrDefault(x => x.Value == guildName).Key;
        if (string.IsNullOrEmpty(groupId))
        {
            Debug.LogError("Guild not found: " + guildName);
            return;
        }

        PlayFabGroupsAPI.ListGroupApplications(new ListGroupApplicationsRequest
        {
            Group = EntityKeyMaker(groupId)
        },
        response => {
            foreach (var application in response.Applications)
            {
                PlayFabGroupsAPI.AcceptGroupApplication(new AcceptGroupApplicationRequest
                {
                    Entity = new EntityKey { Id = application.Entity.Key.Id, Type = application.Entity.Key.Type },
                    Group = EntityKeyMaker(groupId)
                },
                result => Debug.Log("Application Accepted: " + application.Entity.Key.Id),
                error => Debug.LogError("Error accepting application: " + error.GenerateErrorReport()));
            }
        },
        error => Debug.LogError("Error listing applications: " + error.GenerateErrorReport()));
    }

    

}