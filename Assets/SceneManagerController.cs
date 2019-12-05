using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour, IReceive<SignalLoadScene>
{
    private const string loaderSceneName = "loader";
    public static SceneManagerController Instance;    
    
    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void HandleSignal(SignalLoadScene arg)
    {
        if (arg.name == null || arg.name == "") return;


        SceneLoaderProxy.sceneToLoad = arg.name;
        SceneManager.LoadScene(loaderSceneName);
    }

    void OnEnable()
    {
        ProcessSignal.Default.Add(this);
    }
    void OnDisable()
    {
        ProcessSignal.Default.Remove(this);
    }
}
public struct SignalLoadScene { public string name; }