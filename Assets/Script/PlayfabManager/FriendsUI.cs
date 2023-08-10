using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab;

public class FriendsUI : MonoBehaviour
{
    public PlayfabFriends playfabFriends;
    public TMP_InputField friendUsernameInputField;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI removeresultText;
    public TextMeshProUGUI requestText;
    public TextMeshProUGUI friendsListText;
    public GameObject friendsListPanel;
    public GameObject addFriendsPanel;
    public TMP_InputField removeFriendUsernameInputField;
    public GameObject leaderboardPanel; 
    public TextMeshProUGUI leaderboardDisplay;
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

    public void ShowFriends()
    {
        StartCoroutine(playfabFriends.GetFriends(
            friends => {
                string friendsNames = "";
                foreach (var friend in friends)
                {
                    friendsNames += friend.Username + "\n";
                }
                friendsListText.text = friendsNames;
            },
            error => Debug.LogError("Error while checking friends: " + error)
        ));
    }

    public void ShowFriendsListPanel()
    {
        friendsListPanel.SetActive(true);
        addFriendsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        ShowFriends();
    }

    public void ShowAddFriendsPanel()
    {
        friendsListPanel.SetActive(false);
        addFriendsPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        friendsListPanel.SetActive(false);
        addFriendsPanel.SetActive(false);
        GetFriendLeaderboard();
    }
    public void GetFriendLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            new GetLeaderboardAroundPlayerRequest { StatisticName = "HighestPipeScore", MaxResultsCount = 1 },
            playerResult => {
                List<PlayerLeaderboardEntry> allEntries = new List<PlayerLeaderboardEntry>();

            PlayerLeaderboardEntry playerItem = null;
                if (playerResult.Leaderboard.Count > 0)
                {
                    playerItem = playerResult.Leaderboard[0];
                    allEntries.Add(playerItem);
                }

                PlayFabClientAPI.GetFriendLeaderboard(
                    new GetFriendLeaderboardRequest { StatisticName = "HighestPipeScore", MaxResultsCount = 10 },
                    friendResult => {
                        foreach (var item in friendResult.Leaderboard)
                        {

                        if (playerItem == null || item.DisplayName != playerItem.DisplayName)
                            {
                                allEntries.Add(item);
                            }
                        }

                        allEntries.Sort((a, b) => b.StatValue.CompareTo(a.StatValue));

                        leaderboardDisplay.text = "Position - Name - Score\n";
                        int position = 1;
                        foreach (var item in allEntries)
                        {
                            string row = position + " - " + item.DisplayName + " - " + item.StatValue + "\n";
                            leaderboardDisplay.text += row;
                            position++;
                        }
                    },
                    error => {
                        Debug.LogError("Error while retrieving friends leaderboard: " + error.GenerateErrorReport());
                    }
                );
            },
            error => {
                Debug.LogError("Error while retrieving player leaderboard: " + error.GenerateErrorReport());
            }
        );
    }


    public void RemoveFriendByUsername()
    {
        var usernameToRemove = removeFriendUsernameInputField.text;

        if (string.IsNullOrEmpty(usernameToRemove))
        {
            Debug.LogError("Username field is empty!");
            removeresultText.text = "Username field is empty!";
            return;
        }

        StartCoroutine(playfabFriends.GetFriends(
            friends =>
            {
                foreach (var friend in friends)
                {
                    if (friend.Username == usernameToRemove)
                    {
                        StartCoroutine(playfabFriends.RemoveFriend(friend.FriendPlayFabId, result => {
                            removeresultText.text = result;
                            ShowFriends();
                        }, error => {
                            removeresultText.text = "Error removing friend: " + error;
                        }));
                        return;
                    }
                }
                Debug.LogError("Friend not found!");
                removeresultText.text = "Friend not found!";
            },
            error => Debug.LogError("Error while checking friends: " + error)
        ));
    }

}
