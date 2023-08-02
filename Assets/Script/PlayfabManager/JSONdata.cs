//[System.Serializable]
//public class PlayfabData
//{
//    public int TimesPlayed;


//    public PlayfabData(int timesPlayed)
//    {
//        TimesPlayed = timesPlayed;
//    }
//}

[System.Serializable]
public class PlayfabData
{
    public string name;
    public string phoneNumber;
    public string gender;


    public PlayfabData(string _name, string _phoneNumber, string _gender)
    {
        name = _name;
        phoneNumber = _phoneNumber;
        gender = _gender;

    }
}