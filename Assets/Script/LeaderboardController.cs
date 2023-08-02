using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject panel;
    System.Action playerCallback;
    public PlayfabLeaderboard leaderboard;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenPanel(System.Action callBack = null)
    {
        panel.SetActive(true);
        leaderboard.RefreshLeaderboard();
        playerCallback = callBack;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        playerCallback();
        panel.SetActive(false);

        //if (panel.activeSelf)
        //{
        //    panel.SetActive(false);
        //    playerCallback?.Invoke();
        //}
    }
}
