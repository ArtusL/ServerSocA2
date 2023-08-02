using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FriendsUI : MonoBehaviour
{
    public PlayfabFriends playfabFriends;
    public TMP_InputField friendUsernameInputField;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI requestText;

    public void AddFriend()
    {
        var friendUsername = friendUsernameInputField.text;

        if (string.IsNullOrEmpty(friendUsername))
        {
            Debug.LogError("Username field is empty!");
            resultText.text = "Username field is empty!";
            return;
        }

        StartCoroutine(playfabFriends.AddFriendByUsername(friendUsername, result => {
            resultText.text = result;
        }, error => {
            resultText.text = "Error adding friend: " + error;
        }));
    }
    public void CheckFriendRequests()
    {
        StartCoroutine(playfabFriends.CheckFriendRequests(
            friends => {
                foreach (var friend in friends)
                {
                    Debug.Log("Friend: " + friend.Username);
                }
            },
            error => Debug.LogError("Error while checking friend requests: " + error)
        ));
    }

    public void AcceptFriendRequest(string friendPlayfabID)
    {
        Debug.Log("friendPlayfabID: " + friendPlayfabID);

        StartCoroutine(playfabFriends.AddFriend(
            friendPlayfabID,
            result => {
                Debug.Log(result);
                requestText.text = result;
            },
            error => {
                Debug.LogError(error);
                requestText.text = "Error: " + error;
            }
        ));
    }

}
