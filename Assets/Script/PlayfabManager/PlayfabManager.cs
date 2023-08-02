using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class PlayfabManager : MonoBehaviour
{
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "HighestPipeScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnLeaderboardError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("successful leaderboard sent");
    }

    void OnLeaderboardError(PlayFabError error)
    {
        Debug.Log("Error while Logging in");
        Debug.Log(error.GenerateErrorReport());
    }

    public void BackToLogin()
    {
        SceneManager.LoadScene("Login", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Landing");
    }


}
