using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderProxy : MonoBehaviour
{
    public static string sceneToLoad;
    void Start()
    {
        ProcessSignal.Default.Send(new SignalLoaderProgress { progress = 0f });
        sceneLoader = LoadSceneAsync(sceneToLoad);
        StartCoroutine(sceneLoader);
    }
    IEnumerator sceneLoader;
    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        var loading =  SceneManager.LoadSceneAsync(sceneToLoad);
        while (!loading.isDone)
        {
            ProcessSignal.Default.Send(new SignalLoaderProgress { progress = 0f,isComplete=false });
            yield return new WaitForEndOfFrame();
        }
        ProcessSignal.Default.Send(new SignalLoaderProgress { progress = 0f, isComplete = false });
    }


}

