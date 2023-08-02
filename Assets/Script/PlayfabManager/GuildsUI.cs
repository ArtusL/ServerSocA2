using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuildsUI : MonoBehaviour
{
    public PlayfabGuilds playfabGuilds;
    public TMP_InputField guildNameInputField;
    public Button createGuildButton;
    public TMP_Text feedbackText;
    public Button checkGuildButton;

    void Start()
    {
        checkGuildButton.onClick.AddListener(CheckGuildMembership);
        createGuildButton.onClick.AddListener(CreateGuild);
        playfabGuilds.OnGuildFeedback += UpdateFeedback;
    }

    void UpdateFeedback(string feedback)
    {
        feedbackText.text = feedback;
    }

    void OnDestroy()
    {
        playfabGuilds.OnGuildFeedback -= UpdateFeedback;
    }
    void CreateGuild()
    {
        string guildName = guildNameInputField.text;
        string entityId = PlayerPrefs.GetString("TitlePlayFabId");
        Debug.Log("Guild Name: " + guildName);
        Debug.Log("Entity Id: " + entityId);

        if (!string.IsNullOrEmpty(guildName) && !string.IsNullOrEmpty(entityId))
        {
            playfabGuilds.CreateGuild(guildName, entityId);
        }
        else
        {
            Debug.LogError("Guild name or entityId is missing!");
        }
    }
    void CheckGuildMembership()
    {
        string entityId = PlayerPrefs.GetString("TitlePlayFabId");
        playfabGuilds.ListGroups(entityId);
    }
}

