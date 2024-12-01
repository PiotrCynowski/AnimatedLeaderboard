using UnityEngine;

public class LeaderboardEntryData
{
    public string place;
    public string username;
    public string points;
    public Sprite podium;
    public Sprite avatar;
    public Sprite country;
    public Color avatarColor;
    public bool isVip;

    public LeaderboardEntryData(string place, string username, string points, 
        Sprite avatar, Sprite country, Color avatarColor,
        Sprite podium = null, bool isVip = false)
    {
        this.place = place;
        this.username = username;
        this.points = points;
        this.podium = podium ?? default;
        this.avatar = avatar;
        this.country = country;
        this.avatarColor = avatarColor;
        this.isVip = isVip;
    }
}