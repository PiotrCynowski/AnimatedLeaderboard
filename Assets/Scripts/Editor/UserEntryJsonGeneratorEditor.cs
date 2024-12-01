using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using GameManagers;

public class UserEntryJsonGeneratorEditor : EditorWindow
{
    string inputJsonFilePath = "Assets/Resources/data10.json"; 
    int numberOfPlayers = 15;
    bool isAssignJsonToGameManager = true;

    [MenuItem("Tools/User Entry Json Generator")]
    public static void ShowWindow()
    {
        GetWindow<UserEntryJsonGeneratorEditor>("JSON Randomizer");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("JSON Randomizer Tool", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.BeginVertical("box");
        inputJsonFilePath = EditorGUILayout.TextField("Input JSON Path:", inputJsonFilePath);
        numberOfPlayers = EditorGUILayout.IntField("Number of Players:", numberOfPlayers);
        isAssignJsonToGameManager = EditorGUILayout.Toggle("Add JSON to GM", isAssignJsonToGameManager);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate JSON File", GUILayout.Height(40)))
        {
            GenerateRandomJsonFile();
        }
    }

    void GenerateRandomJsonFile()
    {
        if (!File.Exists(inputJsonFilePath))
        {
            Debug.LogError("Input JSON file not found.");
            return;
        }

        if (isAssignJsonToGameManager && GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found in the scene");
            return;
        }

        string customFileName = "data" + numberOfPlayers.ToString() + ".json";
        string outputJsonFilePath = "Assets/Resources/" + customFileName;

        string jsonContent = File.ReadAllText(inputJsonFilePath);
        var baseData = JsonUtility.FromJson<RankingData>(jsonContent);

        var newData = GenerateRandomData(baseData, numberOfPlayers);
        string newJson = JsonUtility.ToJson(newData, true);

        File.WriteAllText(outputJsonFilePath, newJson);
        Debug.Log($"Generated JSON file with {numberOfPlayers} players at {outputJsonFilePath}");

        AssetDatabase.ImportAsset(outputJsonFilePath);
        AssetDatabase.Refresh();

        if (isAssignJsonToGameManager)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("data" + numberOfPlayers);
            if (jsonTextAsset != null)
            {
                GameManager.Instance.jsonFiles.Add(jsonTextAsset);
            }
            else
                Debug.LogError("Failed to load TextAsset from Resources");
        }
    }

    RankingData GenerateRandomData(RankingData baseData, int playerCount)
    {
        RankingData newData = new RankingData
        {
            playerUID = baseData.playerUID, 
            ranking = new List<RankingEntry>()
        };

        for (int i = 0; i < playerCount; i++)
        {
            var newRanking = new RankingEntry
            {
                ranking = i + 1, 
                points = Random.Range(20000, 100000),
                player = new Player
                {
                    uid = System.Guid.NewGuid().ToString(),
                    username = $"Player_{Random.Range(1000, 9999)}",
                    isVip = Random.value > 0.5f,
                    countryCode = GetRandomCountryCode(),
                    characterColor = GetRandomColor(),
                    characterIndex = Random.Range(1, 6)
                }
            };

            newData.ranking.Add(newRanking);
        }

        return newData;
    }

    string GetRandomCountryCode()
    {
        string[] countryCodes = { "US", "BR", "CN", "DE", "ES", "FR", "IT" };
        return countryCodes[Random.Range(0, countryCodes.Length)];
    }

    string GetRandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        return ColorUtility.ToHtmlStringRGB(randomColor);
    }
}