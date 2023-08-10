using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField chatInput;
    public TMP_Text chatDisplay;
    private List<string> chatMessages = new List<string>();
    string playerPlayFabId = PlayFabLogin.PlayFabId;

    void GetPlayerProfile(string playFabId, Action<PlayerProfileModel> callback)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, result =>
        {
            callback.Invoke(result.PlayerProfile);
        }, error =>
        {
            Debug.LogError("Failed to get profile: " + error.GenerateErrorReport());
            callback.Invoke(null);
        });
    }


    public void SendChatMessage()
    {
        string playerPlayFabId = PlayFabLogin.PlayFabId; 
        GetPlayerProfile(playerPlayFabId, profile =>
        {
            string username = profile != null && !string.IsNullOrEmpty(profile.DisplayName)
            ? profile.DisplayName
            : playerPlayFabId;
            photonView.RPC("ReceiveChatMessage", RpcTarget.AllBuffered, username + ": " + chatInput.text);
            chatInput.text = "";
        });
    }

    [PunRPC]
    public void ReceiveChatMessage(string message)
    {
        chatMessages.Add(message);
        if (chatMessages.Count > 10)
        {
            chatMessages.RemoveAt(0);
        }
        chatDisplay.text = string.Join("\n", chatMessages);

        Debug.Log("Received Chat Message: " + message);
    }
}
