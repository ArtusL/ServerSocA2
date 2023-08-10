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
        var request = new AddFriendRequest
        {
            FriendUsername = friendUsername
        };

        bool requestCompleted = false;
        PlayFabClientAPI.AddFriend(request, result =>
        {
            onResult?.Invoke("Friend added successfully!");
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

    public IEnumerator GetFriends(Action<List<FriendInfo>> onResult, Action<string> onError)
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

    public IEnumerator RemoveFriend(string friendPlayfabID, Action<string> onResult, Action<string> onError)
    {
         Debug.Log("Attempting to remove friend with PlayFab ID: " + friendPlayfabID);

         var request = new RemoveFriendRequest
           {
                FriendPlayFabId = friendPlayfabID
         };

         bool requestCompleted = false;

         PlayFabClientAPI.RemoveFriend(request, result =>
         {
             Debug.Log("Successfully removed friend with PlayFab ID: " + friendPlayfabID);
                onResult?.Invoke("Friend removed successfully!");
             requestCompleted = true;
         }, error =>
          {
                Debug.LogError("Error while removing friend: " + error.GenerateErrorReport());
                onError?.Invoke(error.GenerateErrorReport());
                requestCompleted = true;
         });

    yield return new WaitUntil(() => requestCompleted);
    }


}
