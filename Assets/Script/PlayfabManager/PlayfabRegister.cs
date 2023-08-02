using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class PlayfabRegister : MonoBehaviour
{
    public static string PlayFabId { get; private set; } 

    [SerializeField] private TMP_InputField EmailRegisterInput;
    [SerializeField] private TMP_InputField PasswordRegisterInput;
    [SerializeField] private TMP_InputField UsernameRegisterInput;
    [SerializeField] private TMP_Text Messagetxt;
    public PlayfabCurrencies playfabCurrencies;

    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = UsernameRegisterInput.text,
            Email = EmailRegisterInput.text,
            Password = PasswordRegisterInput.text,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while Logging in");
        Messagetxt.text = "Error While Logging in";
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Messagetxt.text = "Registered and Logged In!";

        PlayFabId = result.PlayFabId;
        PlayerPrefs.SetString("PlayFabId", result.PlayFabId);
        SceneManager.LoadScene("Landing");
        DontDestroyOnLoad(this.gameObject);

        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = UsernameRegisterInput.text
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnError);
        playfabCurrencies.GetVirtualCurrencies();
    }


    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name successfully!");
    }


    public void BackToLogin()
    {
        SceneManager.LoadScene("Login");
        SceneManager.UnloadSceneAsync("Register");
    }

    private void OnDestroy()
    {
        Debug.Log("PlayfabRegister is being destroyed");
    }

}
