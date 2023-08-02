using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayfabFriends : MonoBehaviour
{
    public IEnumerator AddFriend(string friendPlayfabID, Action<string> onResult, Action<string> onError)
    {
        Debug.Log("Attempting to add friend with PlayFab ID: " + friendPlayfabID);

        var request = new AddFriendRequest
        {
            FriendPlayFabId = friendPlayfabID
        };

        bool requestCompleted = false;

        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Successfully added friend with PlayFab ID: " + friendPlayfabID);
            onResult?.Invoke("Friend added successfully!");
            requestCompleted = true;
        }, error =>
        {
            Debug.LogError("Error while adding friend: " + error.GenerateErrorReport());
            onError?.Invoke(error.GenerateErrorReport());
            requestCompleted = true;
        });

        yield return new WaitUntil(() => requestCompleted);
    }


    public IEnumerator AddFriendByUsername(string friendUsername, Action<string> onResult, Action<string> onError)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "addFriendByUsername",
            FunctionParameter = new { username = friendUsername },
            GeneratePlayStreamEvent = true,
        };

        bool requestCompleted = false;

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            onResult?.Invoke("Friend request sent successfully!");
            requestCompleted = true;
        }, error =>
        {
            onError?.Invoke(error.GenerateErrorReport());
            requestCompleted = true;
        });

        yield return new WaitUntil(() => requestCompleted);
    }
    public IEnumerator CheckFriendRequests(Action<List<FriendInfo>> onResult, Action<string> onError)
    {
        var request = new GetFriendsListRequest();

        bool requestCompleted = false;

        PlayFabClientAPI.GetFriendsList(request, result =>
        {
            onResult?.Invoke(result.Friends);
            requestCompleted = true;
        }, error =>
        {
            onError?.Invoke(error.GenerateErrorReport());
            requestCompleted = true;
        });

        yield return new WaitUntil(() => requestCompleted);
    }

}
