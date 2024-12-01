using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolSpawn;
using DG.Tweening;

public interface ILeaderboardView
{
    void AddEntriesData(List<LeaderboardEntryData> entries, int playerIndex);
    void ShowLeaderboard();
    void HideLeaderboard();
    public event Action ExitButtonClicked;
}

public class LeaderboardView : LeaderboardBase, ILeaderboardView
{
    [SerializeField] float panelAnimationDuration = 0.2f, entryAnimationDuration = 0.5f;

    ObjectPool<LeaderboardEntryUI> entryPool;
    List<LeaderboardEntryUI> activeEntries;
    List<LeaderboardEntryData> entriesToLoad;
   
    float entryPrefabSizeY, targetPanelSize;
    int maxEntriesBeforeScroll, currentPlayerIndex;

    public event Action ExitButtonClicked;

    public override void ShowLeaderboard()
    {
       base.ShowLeaderboard();
       leaderboardPanel.DOSizeDelta(new Vector2(leaderboardPanel.sizeDelta.x, targetPanelSize), panelAnimationDuration).SetEase(Ease.OutQuad);
       StartCoroutine(RefreshContainer(entriesToLoad));
    }

    public override void HideLeaderboard()
    {
        DOTween.Sequence()
        .Append(leaderboardPanel.DOSizeDelta(new Vector2(leaderboardPanel.sizeDelta.x, 100), panelAnimationDuration).SetEase(Ease.InQuad))
        .Join(DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, panelAnimationDuration).SetEase(Ease.InQuad))
        .OnKill(() => OnLeaderboardHide());
    }
    
    public void Init(int maxEntriesBeforeScroll)
    {
        if (exitButton != null)
            exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());

        entryPool = new ObjectPool<LeaderboardEntryUI>(entryPrefab, leaderboardContainer);
        activeEntries = new();

        this.maxEntriesBeforeScroll = maxEntriesBeforeScroll;

        entryPrefabSizeY = entryPrefab.GetComponent<RectTransform>().rect.height;

        ResetPanelSize();
    }

    public void AddEntriesData(List<LeaderboardEntryData> entries, int playerIndex)
    {
        foreach (var activeEntry in activeEntries)
            entryPool.ReturnToPool(activeEntry);

        activeEntries.Clear();
        playerEntryPrefab.gameObject.SetActive(false);

        currentPlayerIndex = playerIndex;
        entriesToLoad = entries;
        AdjustPanelSize(entries.Count);
    }

    void OnLeaderboardHide()
    {
        leaderboardPanel.gameObject.SetActive(false);
        ResetPanelSize();
    }

    #region Entries
    IEnumerator RefreshContainer(List<LeaderboardEntryData> entries)
    {
        LeaderboardEntryUI entryUI;
        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            if (i == currentPlayerIndex) {
                entryUI = playerEntryPrefab;
                entryUI.transform.SetAsLastSibling();
                entryUI.gameObject.SetActive(true);
            }
            else
            {
                entryUI = entryPool.Get();
                activeEntries.Add(entryUI);
            }

            entryUI.SetEntry(entry);
            AnimateEntry(entryUI.transform, entryUI.canvasGroup);

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    void AnimateEntry(Transform entryTransform, CanvasGroup entryCanvasGroup)
    {
        entryTransform.localScale = Vector3.zero;
        var sequence = DOTween.Sequence();
        sequence.Append(entryTransform.DOScale(Vector3.one, entryAnimationDuration).SetEase(Ease.OutBack));

        if (entryCanvasGroup == null)
            return;
        entryCanvasGroup.alpha = 0;
        sequence.Join(DOTween.To(() => entryCanvasGroup.alpha, x => entryCanvasGroup.alpha = x, 1f, entryAnimationDuration).SetEase(Ease.OutQuad));      
    }
   

    void AdjustPanelSize(int entries)
    {
        targetPanelSize = -containerViewport.anchoredPosition.y +
            (entries > maxEntriesBeforeScroll ? maxEntriesBeforeScroll : entries + 1) * entryPrefabSizeY;

        scrollRect.enabled = entries > maxEntriesBeforeScroll;
    }
    #endregion
}