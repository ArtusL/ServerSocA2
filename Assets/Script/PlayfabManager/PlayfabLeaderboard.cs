using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class PlayfabLeaderboard : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowsParent;
    [SerializeField] private Button RefreshLeaderBoardBTN;

    public PlayfabManager playfabManager;

    private void Start()
    {
        RefreshLeaderBoardBTN.onClick.AddListener(() => RefreshLeaderboard());
    }
    private void Update()
    {
        RefreshLeaderBoardBTN.onClick.AddListener(() =>

        {
            RefreshLeaderboard();
        });
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while Logging in");
        Debug.Log(error.GenerateErrorReport());
    }

    public void RefreshLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighestPipeScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            var item = result.Leaderboard[i];
            StartCoroutine(GetPlayerProfileAndDisplay(item, i, result.Leaderboard.Count));
        }
        foreach (Transform child in rowsParent)
        {
            child.SetAsFirstSibling();
        }
    }



    IEnumerator GetPlayerProfileAndDisplay(PlayerLeaderboardEntry item, int index, int count)
    {
        bool finishedLoading = false;
        PlayerProfileModel profile = null;

        GetPlayerProfile(item.PlayFabId, (retrievedProfile) => {
            profile = retrievedProfile;
            finishedLoading = true;
        });

        while (!finishedLoading)
        {
            yield return null;
        }

        GameObject newGo = Instantiate(rowPrefab, rowsParent);
        newGo.transform.SetSiblingIndex(count - index - 1);
        Text[] texts = newGo.GetComponentsInChildren<Text>();
        RectTransform rectTransform = newGo.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = Vector3.zero;

        string displayName = profile != null && !string.IsNullOrEmpty(profile.DisplayName)
            ? profile.DisplayName
            : item.PlayFabId;

        if (texts.Length >= 3)
        {
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = displayName;
            texts[2].text = item.StatValue.ToString();
        }

        Debug.Log($"Name: {displayName}, Position: {item.Position}, Score: {item.StatValue}");
    }



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

        PlayFabClientAPI.GetPlayerProfile(request, (result) =>
        {
            callback.Invoke(result.PlayerProfile);
        }, OnError);
    }
}
