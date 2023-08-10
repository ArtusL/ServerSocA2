using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GuildsUI : MonoBehaviour
{
    public PlayfabGuilds playfabGuilds;

    public TMP_InputField guildNameInputField;
    public Button createGuildButton;
    public TMP_Text feedbackText;

    public Button checkGuildButton;
    public TextMeshProUGUI guildsDisplayText;

    public TMP_InputField joinGuildNameInputField;
    public Button joinGuildButton;

    public TMP_InputField applicantNameInputField; 
    public Button acceptApplicationButton;
    public TMP_InputField guildNameForMemberInputField; 
    public Button checkGuildMembersButton; 
    public TextMeshProUGUI membersDisplayText; 
    public TextMeshProUGUI applicationsDisplayText;

    public GameObject joinCreatePanel; 
    public GameObject yourGuildsPanel;
    public GameObject membersPanel;

    public Button showJoinCreateButton;
    public Button ShowMembersButton;
    public Button showYourGuildsButton;

    public TMP_InputField guildNameForApplicationsInputField;
    public Button acceptAllApplicationsButton;

    public TMP_InputField leaveGuildNameInputField;
    public Button leaveGuildButton;
    public TMP_Text LeavefeedbackText;

    void Start()
    {
        checkGuildButton.onClick.AddListener(CheckGuildMembership);
        createGuildButton.onClick.AddListener(CreateGuild);
        playfabGuilds.OnGuildFeedback += UpdateFeedback;
        playfabGuilds.OnGuildListUpdated += DisplayPlayerGuilds;
        joinGuildButton.onClick.AddListener(JoinGuildById);
        //playfabGuilds.OnApplicationListUpdated += DisplayPlayerApplications; 
        //acceptApplicationButton.onClick.AddListener(AcceptApplication);
        playfabGuilds.OnMemberListUpdated += DisplayPlayerMembers;
        leaveGuildButton.onClick.AddListener(LeaveGuildById);

        checkGuildMembersButton.onClick.AddListener(CheckGuildMembers);
        showJoinCreateButton.onClick.AddListener(ShowJoinCreatePanel);
        ShowMembersButton.onClick.AddListener(ShowMembersPanel);
        showYourGuildsButton.onClick.AddListener(ShowYourGuildsPanel);

        acceptAllApplicationsButton.onClick.AddListener(AcceptAllApplications);


        ShowJoinCreatePanel();
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
    public void ShowJoinCreatePanel()
    {
        joinCreatePanel.SetActive(true);
        yourGuildsPanel.SetActive(false);
        membersPanel.SetActive(false);
    }

    public void ShowYourGuildsPanel()
    {
        joinCreatePanel.SetActive(false);
        yourGuildsPanel.SetActive(true);
        membersPanel.SetActive(false);
        CheckGuildMembership();
    }

    public void ShowMembersPanel()
    {
        membersPanel.SetActive(true);
        joinCreatePanel.SetActive(false);
        yourGuildsPanel.SetActive(false);
    }

    void CheckGuildMembership()
    {
        string entityId = PlayerPrefs.GetString("TitlePlayFabId");
        playfabGuilds.ListGroups(entityId);
    }

    public void DisplayPlayerGuilds(List<string> guilds)
    {
        guildsDisplayText.text = "";

        foreach (string guild in guilds)
        {
            guildsDisplayText.text += guild + "\n";
        }
    }

    //void JoinGuildByName()
    //{
    //    string guildName = joinGuildNameInputField.text;
    //    string entityId = PlayerPrefs.GetString("TitlePlayFabId");

    //    if (!string.IsNullOrEmpty(guildName) && !string.IsNullOrEmpty(entityId))
    //    {
    //        playfabGuilds.JoinGuild(guildName, entityId);
    //    }
    //    else
    //    {
    //        feedbackText.text = "Please enter a valid guild name.";
    //    }
    //}

    void JoinGuildById()
    {
        string guildId = joinGuildNameInputField.text;
        string entityId = PlayerPrefs.GetString("TitlePlayFabId");

        if (!string.IsNullOrEmpty(guildId) && !string.IsNullOrEmpty(entityId))
        {
            playfabGuilds.JoinGuildById(guildId, entityId); 
        }
        else
        {
            feedbackText.text = "Please enter a valid guild ID.";
        }
    }

    public void DisplayPlayerMembers(List<string> members)
    {
        membersDisplayText.text = "";
        foreach (string member in members)
        {
            membersDisplayText.text += member + "\n";
        }
    }

    void CheckGuildMembers()
    {
        string guildName = guildNameForMemberInputField.text;

        string groupId = playfabGuilds.GroupNameById.FirstOrDefault(x => x.Value == guildName).Key;

        if (!string.IsNullOrEmpty(groupId))
        {
            playfabGuilds.ListGroupMembers(groupId);
        }
        else
        {
            feedbackText.text = "Please enter a valid guild name.";
        }
    }

    public void AcceptAllApplications()
    {
        string guildName = guildNameForApplicationsInputField.text;
        if (!string.IsNullOrEmpty(guildName))
        {
            playfabGuilds.AcceptAllApplications(guildName);
        }
        else
        {
            feedbackText.text = "Please enter a valid guild name.";
        }
    }

    void LeaveGuildById()
    {
        string guildName = leaveGuildNameInputField.text;
        string groupId = playfabGuilds.GroupNameById.FirstOrDefault(x => x.Value == guildName).Key;
        string entityId = PlayerPrefs.GetString("TitlePlayFabId");

        if (!string.IsNullOrEmpty(groupId) && !string.IsNullOrEmpty(entityId))
        {
            playfabGuilds.LeaveGuildById(groupId, entityId);
            LeavefeedbackText.text = "Successfully Left Guild";
            CheckGuildMembership();
        }
        else
        {
            LeavefeedbackText.text = "Please enter a valid guild name.";
        }
    }
}