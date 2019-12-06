using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text messagePrefab;

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
