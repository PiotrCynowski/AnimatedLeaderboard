using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public string uid;
    public string username;
    public bool isVip;
    public string countryCode;
    public string characterColor;
    public int characterIndex;
}

[System.Serializable]
public class RankingEntry
{
    public Player player;
    public int ranking;
    public int points;
}

[System.Serializable]
public class RankingData
{
    public List<RankingEntry> ranking;
    public string playerUID;
}