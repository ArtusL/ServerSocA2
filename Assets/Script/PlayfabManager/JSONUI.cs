using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class JSONUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI phoneNumberText;
    public TextMeshProUGUI genderText;

    public void RetrievePlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnUserDataRetrieved, OnError);
    }

 
     private void OnUserDataRetrieved(GetUserDataResult result)
     {
         if (result.Data.TryGetValue("PlayerData", out var userDataRecord))
         {
                string jsonData = userDataRecord.Value;
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
                nameText.text = "Name: " + playerData.Name;
                phoneNumberText.text = "Phone Number: " + playerData.PhoneNumber;
                genderText.text = "Gender: " + playerData.Gender;
         }
         else
         {
                nameText.text = "Name: N/A";
                phoneNumberText.text = "Phone Number: N/A";
                genderText.text = "Gender: N/A";
         }
     }
    

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Get user data error: " + error.ErrorMessage);
    }
}
