
//using UnityEngine;
//using PlayFab;
//using PlayFab.ClientModels;
//using System.Collections.Generic;
//using TMPro;
//public class PlayfabJSON : MonoBehaviour
//{
//    private GameController gameController;
//    [SerializeField] private TMP_Text TimePlayedtext;
//    private void Awake()
//    {
//        gameController = FindObjectOfType<GameController>();
//    }
//    public void Update()
//    {
//        TimePlayedtext.text = "Times Played: " + GetTimesPlayed();
//    }
//    public void PlayGame()
//    {

//        gameController.timesPlayed++;


//        string jsonData = JsonUtility.ToJson(new PlayfabData(gameController.timesPlayed));


//        UpdateUserDataRequest request = new UpdateUserDataRequest()
//        {
//            Data = new Dictionary<string, string>()
//            {
//                { "TimesPlayed", jsonData }
//            }
//        };
//        PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdated, OnError);
//    }

//    private void OnUserDataUpdated(UpdateUserDataResult result)
//    {
//        Debug.Log("User data updated successfully");
//    }

//    private void OnError(PlayFabError error)
//    {
//        Debug.LogError("Update user data error: " + error.ErrorMessage);
//    }

//    public int GetTimesPlayed()
//    {
//        return gameController.timesPlayed;
//    }
//}
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using PlayFab;
//using PlayFab.ClientModels;
//using System.Collections.Generic;

//public class PlayfabJSON : MonoBehaviour
//{
//    public TMP_InputField nameInputField;
//    public TMP_InputField phoneNumberInputField;
//    public TMP_Dropdown genderDropdown;

//    public TextMeshProUGUI statusText;

//    public void SendPlayerData()
//    {
//        string name = nameInputField.text;
//        string phoneNumber = phoneNumberInputField.text;
//        string gender = genderDropdown.options[genderDropdown.value].text;


//        Dictionary<string, string> playerData = new Dictionary<string, string>();
//        playerData.Add("Name", name);
//        playerData.Add("PhoneNumber", phoneNumber);
//        playerData.Add("Gender", gender);

//        //List<PlayerData> PlayerDataList= new List<PlayerData>();

//        string jsonData = JsonUtility.ToJson(playerData);
//        UpdateUserDataRequest request = new UpdateUserDataRequest()
//        {
//            Data = new Dictionary<string, string>()
//            {
//                { "PlayerData", jsonData }
//            }
//        };
//        PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdated, OnError);

//        // Clear the input fields
//        nameInputField.text = "";
//        phoneNumberInputField.text = "";
//        genderDropdown.value = 0;

//        // Display status message
//        statusText.text = "Player data sent to PlayFab!";
//    }

//    private void OnUserDataUpdated(UpdateUserDataResult result)
//    {
//        Debug.Log("User data updated successfully");
//    }

//    private void OnError(PlayFabError error)
//    {
//        Debug.LogError("Update user data error: " + error.ErrorMessage);
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public string PhoneNumber;
    public string Gender;
}

public class PlayfabJSON : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField phoneNumberInputField;
    public TMP_Dropdown genderDropdown;

    public TextMeshProUGUI statusText;

    public void SendPlayerData()
    {
        string name = nameInputField.text;
        string phoneNumber = phoneNumberInputField.text;
        string gender = genderDropdown.options[genderDropdown.value].text;

        PlayerData playerData = new PlayerData()
        {
            Name = name,
            PhoneNumber = phoneNumber,
            Gender = gender
        };

        string jsonData = JsonUtility.ToJson(playerData);
        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { "PlayerData", jsonData }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdated, OnError);

        // Clear the input fields
        nameInputField.text = "";
        phoneNumberInputField.text = "";
        genderDropdown.value = 0;

        // Display status message
        statusText.text = "Player data sent to PlayFab!";
    }

    private void OnUserDataUpdated(UpdateUserDataResult result)
    {
        Debug.Log("User data updated successfully");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Update user data error: " + error.ErrorMessage);
    }
}
