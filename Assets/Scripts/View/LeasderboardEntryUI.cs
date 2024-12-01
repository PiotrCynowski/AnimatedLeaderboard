using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] Text place;
    [SerializeField] Text username;   
    [SerializeField] Text points;
    [SerializeField] Image podium;
    [SerializeField] Image avatar;
    [SerializeField] Image avatarBackground;
    [SerializeField] Image country;
    [SerializeField] Image vipStatus;
    public CanvasGroup canvasGroup;

    public void SetEntry(LeaderboardEntryData data)
    {
        this.place.text = data.place;
        this.username.text = data.username;
        this.points.text = data.points;
        if (data.podium != null)
        {
            this.podium.sprite = data.podium;
            this.podium.enabled = true;
        }
        else
        {
            this.podium.enabled = false;
        }
        this.avatar.sprite = data.avatar;
        this.country.sprite = data.country;
        this.avatarBackground.color = data.avatarColor;
        this.vipStatus.gameObject.SetActive(data.isVip);
    }
}