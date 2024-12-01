using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardSpriteSelector", menuName = "ScriptableObjects/LeaderboardSpriteSelector", order = 0)]
public class LeaderboardSpriteSelector : ScriptableObject
{
    public AssetSprites[] assetSpritesUi;
    public Sprite defaultSprite;

    [Serializable]
   public class AssetSprites
    {
        public string id;
        public Sprite sprite;
    }

    public Sprite GetSprite(string id)
    {
        AssetSprites assetSprite = Array.Find(assetSpritesUi, sprite => sprite.id == id);
        if (assetSprite != null && assetSprite.sprite != null)
        {
            return assetSprite.sprite; 
        }
        Debug.LogWarning($"Sprite with id'{id}' not found.");
        return defaultSprite;
    }

    public Sprite GetSprite(int index)
    {
        if (index >= 0 && index < assetSpritesUi.Length)
        {
            return assetSpritesUi[index].sprite;
        }
        Debug.LogWarning($"Sprite with index '{index}' not found.");
        return defaultSprite;
    }
}