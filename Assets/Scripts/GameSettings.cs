using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;
    public Data data;
    private void Awake()
    {
        instance = this;
    }
}
