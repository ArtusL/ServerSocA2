using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject gameOverObject;
    public PlayfabCurrencies playfabCurrencies;
    //public PlayfabJSON dataSender;

    //public PlayfabJSON playfabJSON;
    //public int timesPlayed = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI FinalScoreText;


    public int difficultyMax = 5;

    [HideInInspector]
    public bool isGameOver = false;
    public float scrollSpeed = -2.5f;

    public int columnScore = 1;
    private int score = 0;
    private int highestScore = 0;

    public PlayfabManager playfabManager;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        //LoadHighScore();
        scoreText.text = "0";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetGame()
    {
        //Reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLanding()
    {
        SceneManager.LoadScene("Landing");
        
    }

    public void Scored(int value)
    {
        //Check if it is game over
        if (isGameOver)
            return;

        score += value;
        scoreText.text = score.ToString();
        FinalScoreText.text = score.ToString();
        if (score >= highestScore)
        {
            SaveHighScore(score);
        }
        playfabCurrencies.AddCoinsCurrency();
    }

    private void SaveHighScore(int score)
    {
        //highestScore = score;
        //PlayerPrefs.SetInt("highestScore", highestScore);
        //highScoreText.text = highestScore.ToString();
        highestScore = score;
        highScoreText.text = highestScore.ToString();
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = highestScore
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUserDataUpdated, OnError);
    }
    //UpdateUserDataRequest request = new UpdateUserDataRequest()
    //    {
    //        Data = new Dictionary<string, string>()
    //        {
    //            { "HighScore", highestScore.ToString() }
    //        }
    //    };
    //    PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdated, OnError);
    //}

    //private void LoadHighScore()
    //{
    //    if (PlayerPrefs.HasKey("highestScore"))
    //    {
    //        highestScore = PlayerPrefs.GetInt("highestScore");
    //        highScoreText.text = highestScore.ToString();
    //    }
    //    else
    //    {
    //        GetUserDataRequest request = new GetUserDataRequest();
    //        PlayFabClientAPI.GetUserData(request, OnUserDataReceived, OnError);
    //    }
    //}

    //private void OnUserDataReceived(GetUserDataResult result)
    //{
    //    if (result.Data.TryGetValue("HighScore", out var highScoreValue))
    //    {
    //        highestScore = int.Parse(highScoreValue.Value);
    //        highScoreText.text = highestScore.ToString();
    //    }
    //}
    public void GameOver()
    {
        gameOverObject.SetActive(true);
        isGameOver = true;
        playfabManager.SendLeaderboard(highestScore);
    }

    private void OnUserDataUpdated(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("User data updated successfully");
    }
    
    private void OnError(PlayFabError error)
    {
        Debug.LogError("Update user data error: " + error.ErrorMessage);
    }
}
