using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;

public class PlayfabCurrencies : MonoBehaviour
{
    //public static PlayfabCurrencies instance;
    //public GameObject gameOverObject;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI InventoryText;
    

    public delegate void CoinCountUpdatedEvent(int coinCount);
    public static event CoinCountUpdatedEvent OnCoinCountUpdated;
    void Awake()
    {
        
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetVirtualcurrenciesSuccess, OnError);
    }

    public void OnGetVirtualcurrenciesSuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency["CN"];
        OnCoinCountUpdated?.Invoke(coins);
    }
    public void GetPlayerInventory()
    {

        var userInventoryRequest = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(userInventoryRequest, result =>
        {
            List<ItemInstance> itemInstances = result.Inventory;
            InventoryText.text += "Player Inventory : "+ '\n';
            foreach (ItemInstance itemInstance in itemInstances)
            {
                InventoryText.text += itemInstance.DisplayName + " - " + itemInstance.RemainingUses + '\n';
            }
        }, OnError);
    }

    public void BuyMedal()
    {
        PurchaseItem("Medal", 10);
        GetVirtualCurrencies();
    }

    public void BuyRuby()
    {
        PurchaseItem("Ruby", 20);
        GetVirtualCurrencies();
    }

    public void BuyTrophy()
    {
        PurchaseItem("Trophy", 30);
        GetVirtualCurrencies();
    }

    private void PurchaseItem(string itemName, int price)
    {
        var purchaseRequest = new PurchaseItemRequest
        {
            CatalogVersion = "Awards",
            ItemId = itemName,
            VirtualCurrency = "CN",
            Price = price
        };

        PlayFabClientAPI.PurchaseItem(purchaseRequest, result =>
        {
            messageText.text = "Purchased " + itemName + " successfully!";
        }, OnError);
    }

    public void AddCoinsCurrency()
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = 1
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddCoinsSuccess, OnError);
    }

    private void OnAddCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Added coins");
    }
}