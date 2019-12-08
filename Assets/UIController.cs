using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text messagePrefab;
    [SerializeField] RectTransform progressBarFill;
    [SerializeField] RectTransform rank;
    [SerializeField] RectTransform menu_rank;
    RectTransform progressBarContainer;

    private void Awake()
    {
        progressBarContainer = progressBarFill.parent as RectTransform;
        SetProgress(0.0f);
    }
    public void SetProgress(float progress)
    {
        float width = progressBarContainer.sizeDelta.x;
        progressBarFill.offsetMax = new Vector2(- width + width * progress, progressBarFill.offsetMax.y);
    }
    public void UpdateRank(Dictionary<CharController, int> charsScoreData)
    {
        UpdateRank(charsScoreData,  rank);
    }
    public void UpdateRankMenu(Dictionary<CharController, int> charsScoreData)
    {
        menu_rank.gameObject.SetActive(true);
        UpdateRank(charsScoreData, menu_rank);
    }
    private void UpdateRank(Dictionary<CharController, int> charsScoreData, RectTransform rank)
    {
        TMPro.TMP_Text[] texts = rank.GetComponentsInChildren<TMPro.TMP_Text>();
        var myList = charsScoreData.ToList();

        myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        
        int i = 0;
        foreach (var item in myList)
        {
            if (i == 0)
                ProcessSignal.Default.Send(new SignalFirstRank { charController = item.Key });
            
            texts[i].text =(i+1).ToString()+" " + item.Key.GetName();
            i++;
        }
        
    }
    internal void SpawnMessage(string messageText)
    {
        TMPro.TMP_Text mes = Instantiate(messagePrefab, transform);
        mes.text = messageText;
        Vector2 targetPos = mes.rectTransform.anchoredPosition + new Vector2(30f, 30f);

        Sequence messageSequence = DOTween.Sequence();

        messageSequence.Append(mes.rectTransform.DOAnchorPos(targetPos, 1f))
          .Insert(0.5f, mes.DOFade(0, 0.5f)).OnComplete(delegate
          {
              SimplePool.Despawn(mes.gameObject);
          });
    }

}
