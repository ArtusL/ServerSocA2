//using UnityEngine;
//using UnityEngine.UI;

//public class GuildButton : MonoBehaviour
//{
//    public Button button;
//    public Text guildNameText;
//    private PlayfabGuilds playfabGuilds;

//    private string guildId;
//    private string guildName;

//    public void SetGuild(string guildId, string guildName)
//    {
//        this.guildId = guildId;
//        this.guildName = guildName;
//        guildNameText.text = guildName;
//    }

//    public void OnClick()
//    {
//        playfabGuilds.ListGroupMembers(guildId);
//    }

//    void Start()
//    {
//        button.onClick.AddListener(OnClick);
//        playfabGuilds.OnGuildMembersListed += UpdateGuildMembersText;
//    }

//    void OnDestroy()
//    {
//        playfabGuilds.OnGuildMembersListed -= UpdateGuildMembersText;
//    }

//    void UpdateGuildMembersText(string membersList)
//    {
//        // Update some text field in the UI
//    }
//}
