using UnityEngine;
using UnityEngine.UI;

public abstract class LeaderboardBase : MonoBehaviour
{
    [SerializeField] protected RectTransform leaderboardPanel, leaderboardContainer, containerViewport;
    [SerializeField] protected LeaderboardEntryUI entryPrefab, playerEntryPrefab;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Button exitButton;

    [SerializeField] float emptyPanelSize = 100f;

    protected ScrollRect scrollRect;
  
    public void Awake()
    {
        scrollRect = leaderboardPanel.GetComponent<ScrollRect>();
    }

    public virtual void ShowLeaderboard()
    {
        canvasGroup.alpha = 1f;
        leaderboardPanel.gameObject.SetActive(true);
    }
   
    public abstract void HideLeaderboard();

    public void ResetPanelSize()
    {
        leaderboardPanel.sizeDelta = new Vector2(leaderboardPanel.sizeDelta.x, emptyPanelSize);
    }
}