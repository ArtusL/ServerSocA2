using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI messageText;
    public PlayfabCurrencies playfabCurrencies;
    [SerializeField] private TextMeshProUGUI CoinsTxt;

    private System.Action playerCallback;

    private bool retreiveVirtCurrency = false;
    public void OpenPanel(System.Action callBack = null)
    {
        panel.SetActive(true);
        playerCallback = callBack;
        messageText.text = "";
        if (!retreiveVirtCurrency)
        {
            GetVirtualCurrencies();
            retreiveVirtCurrency = true;
        }
        GetVirtualCurrencies();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        playerCallback?.Invoke();
    }
    private void OnEnable()
    {
        PlayfabCurrencies.OnCoinCountUpdated += UpdateCoinsText;
    }

    private void OnDisable()
    {
        PlayfabCurrencies.OnCoinCountUpdated -= UpdateCoinsText;
    }
    private void UpdateCoinsText(int coinCount)
    {
        CoinsTxt.text = "Coins: " + coinCount.ToString();
    }
    public void GetVirtualCurrencies()
    {
        playfabCurrencies.GetVirtualCurrencies ();
    }

    public void BuyMedal()
    {
        playfabCurrencies.BuyMedal();
    }

    public void BuyRuby()
    {
        playfabCurrencies.BuyRuby();
    }

    public void BuyTrophy()
    {
        playfabCurrencies.BuyTrophy();
    }

}
