using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPresenter
{
    readonly LeaderboardData model;
    readonly ILeaderboardView view;
    readonly LeaderboardSpriteSelector flagsSelector;
    readonly LeaderboardSpriteSelector avatarSelector;
    readonly LeaderboardSpriteSelector podiumSelector;

    public LeaderboardPresenter(
        LeaderboardData model, 
        ILeaderboardView view, 
        LeaderboardSpriteSelector flagsSelector, 
        LeaderboardSpriteSelector avatarSelector, 
        LeaderboardSpriteSelector podiumSelector)
    {
        this.model = model;
        this.view = view;
        this.flagsSelector = flagsSelector;
        this.avatarSelector = avatarSelector;
        this.podiumSelector = podiumSelector;

        view.ExitButtonClicked += HandleExitButtonClicked;
    }

    public void HandleTapScreen()
    {
        view.ShowLeaderboard();
    }

    public void LoadData(string jsonData)
    {
        model.LoadFromJson(jsonData);
        var entries = MapRankingToEntryData(model.GetEntries());
        view.AddEntriesData(entries, RandomlySelectUserPlayer(entries.Count));
    }

    public List<LeaderboardEntryData> MapRankingToEntryData(List<RankingEntry> entry)
    {
        List<LeaderboardEntryData> data = new();

        Player player;
        Sprite podium, country, avatar;
        LeaderboardEntryData newData;
        Color avatarBg;
        foreach (var entryData in entry)
        {
            player = entryData.player;
            podium = podiumSelector.GetSprite(entryData.ranking.ToString());
            country = flagsSelector.GetSprite(entryData.player.countryCode);
            avatar = avatarSelector.GetSprite(entryData.player.characterIndex);
            avatarBg = GetColor(player.characterColor);

            newData = new(entryData.ranking.ToString(), player.username, entryData.points.ToString(), 
                avatar, country, avatarBg, podium, player.isVip);
            data.Add(newData);
        }
        return data;
    }

    Color GetColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out Color color))
            return color;
        Debug.LogWarning($"Invalid color string: {colorString}. Using default color.");
        return Color.white;
    }

    int RandomlySelectUserPlayer(int allEntries)
    {
        return Random.Range(0, allEntries);
    }

    void HandleExitButtonClicked()
    {
        view.HideLeaderboard();
    }
}