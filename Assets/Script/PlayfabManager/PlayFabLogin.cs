using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using PlayFab.AuthenticationModels;

public class PlayFabLogin : MonoBehaviour
{
    public static string PlayFabId;

    [SerializeField] private TMP_InputField EmailInput;
    [SerializeField] private TMP_InputField PasswordInput;
    [SerializeField] private TMP_Text Messagetxt;

    public PlayfabCurrencies playfabCurrencies;

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = EmailInput.text,
            Password = PasswordInput.text,

             InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
             {
                 GetPlayerProfile = true
             }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        
    }
    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while Logging in");
        Messagetxt.text = "Error While Logging in, check if email and or password is correct";
        Debug.Log(error.GenerateErrorReport());
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Messagetxt.text = "Logged In!";
        PlayFabId = result.PlayFabId;
        PlayerPrefs.SetString("MasterPlayFabId", result.PlayFabId);
        SceneManager.LoadScene("Landing");

        PlayFabAuthenticationAPI.GetEntityToken(new GetEntityTokenRequest(), OnGetEntityTokenSuccess, OnGetEntityTokenError);
    }

    public void OnGetEntityTokenSuccess(GetEntityTokenResponse result)
    {
        string titlePlayerAccountId = result.Entity.Id;
        PlayerPrefs.SetString("TitlePlayFabId", titlePlayerAccountId);
        Debug.Log("Saved TitlePlayerAccountId: " + titlePlayerAccountId);
    }

    public void OnGetEntityTokenError(PlayFabError error)
    {
        Debug.LogError("Failed to get EntityToken: " + error.GenerateErrorReport());
    }


    public void GuestLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnGuestSuccess, OnError);

    }

    private void OnGuestSuccess(LoginResult result)
    {
        PlayFabLogin.PlayFabId = result.PlayFabId; 
        Messagetxt.text = "Guest Logged In!";
        SceneManager.LoadScene("Landing");
    }

    public void BackToRegister()
    {
        SceneManager.LoadScene("Register");
        SceneManager.UnloadSceneAsync("Login");
    }
}

