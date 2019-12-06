using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingProgressView : MonoBehaviour, IReceive<SignalLoaderProgress>
{
    [SerializeField] TMP_Text progressText;

public void HandleSignal(SignalLoaderProgress arg)
    {
        UpdateLoadProgress(arg.progress);
    }

    private void UpdateLoadProgress(float progress)
    {
        progressText.text = "Loading " + ((int)(progress * 100)).ToString() + "%";
    }
}
