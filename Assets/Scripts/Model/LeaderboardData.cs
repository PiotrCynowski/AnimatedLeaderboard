using System.Collections.Generic;
using UnityEngine;

public class LeaderboardData
{
    List<RankingEntry> leaderboardEntries;

    public LeaderboardData()
    {
        leaderboardEntries = new List<RankingEntry>();
    }

    public void LoadFromJson(string jsonData)
    {
        var data = JsonUtility.FromJson<RankingData>(jsonData);
        leaderboardEntries = data.ranking;
    }

    public List<RankingEntry> GetEntries()
    {
        return leaderboardEntries;
    }
}