using PlayFab;
using PlayFab.GroupsModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabGuilds : MonoBehaviour
{
    public Action<string> OnGuildFeedback;

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
        var request = new ListMembershipRequest { Entity = EntityKeyMaker(entityId) };
        PlayFabGroupsAPI.ListMembership(request, OnListGroups, OnSharedError);
    }

    private void OnListGroups(ListMembershipResponse response)
    {
        var prevRequest = (ListMembershipRequest)response.Request;
        foreach (var pair in response.Groups)
        {
            GroupNameById[pair.Group.Id] = pair.GroupName;
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, pair.Group.Id));
        }

        string feedback = "You are a member of: ";
        foreach (var pair in EntityGroupPairs)
        {
            if (pair.Key == prevRequest.Entity.Id)
            {
                feedback += GroupNameById[pair.Value] + ", ";
            }
        }

        OnGuildFeedback?.Invoke(feedback);
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
}
