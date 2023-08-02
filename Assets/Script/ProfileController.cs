

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileController : MonoBehaviour
{
    public PlayfabCurrencies playfabCurrencies;
    public GameObject panel;

    System.Action playerCallback;
    [SerializeField] private Button openPanelButton;
    [SerializeField] private Button backButton;

    //public TextMeshProUGUI timesPlayedText; 

    private bool isInventoryRetrieved = false;

    private GameController gameController; 

    void Start()
    {
        panel.SetActive(false);

        gameController = FindObjectOfType<GameController>();

        //timesPlayedText.text = gameController.timesPlayed.ToString();
    }

    void Update()
    {
        openPanelButton.onClick.AddListener(() =>
        {
            OpenPanel();
        });

        backButton.onClick.AddListener(() =>
        {
            ClosePanel();
        });
    }

    public void OpenPanel(System.Action callBack = null)
    {
        panel.SetActive(true);
        playerCallback = callBack;

        if (!isInventoryRetrieved)
        {
            GetPlayerInventory();
            isInventoryRetrieved = true;
        }

        //timesPlayedText.text = gameController.timesPlayed.ToString();
    }

    public void GetPlayerInventory()
    {
        playfabCurrencies.GetPlayerInventory();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        if (playerCallback != null)
        {
            playerCallback();
        }
    }
}