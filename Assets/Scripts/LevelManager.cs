using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LevelManager : MonoBehaviour,IReceive<SignalHitPad>, IReceive<SignalGameover>
{
    [SerializeField] Transform wayPointPadsContainer;
    [SerializeField] CharController playerPrefab;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] UIController uiController;
    
    Pad[] pads;

    private void Awake()
    {
        InitLevel();
    }
    private void Start()
    {
        SpawnPlayer();
    }
    void InitPads()
    {
        if (wayPointPadsContainer == null)
        {
            Debug.LogError("Set way pad container");
            return;
        }

        pads = wayPointPadsContainer.GetComponentsInChildren<Pad>();

        if (pads.Length == 0)
        {
            Debug.LogError("No pads found");
            return;
        }

        for (int i = 0; i < pads.Length; i++)
        {
            pads[i].InitNumber( i+1);
        }
    }
    void SpawnMessage(string messageText)
    {
        uiController.SpawnMessage(messageText);


    }
    public void InitLevel()
    {
        InitPads();
    }

    private void DeReset()
    {
        for (int i = 0; i < pads.Length; i++)
        {
            pads[i].DoReset();
        }
    }

    CharController player;
    public void SpawnPlayer()
    {
        if (player != null)
            Destroy(player.gameObject);

        player = Instantiate(playerPrefab);
        player.isPlayer = true;
        

        player.transform.position = pads[pads.Length - 1].transform.position + Vector3.up * 10f;

        Vector3 deltaPositions = pads[pads.Length - 2].transform.position - pads[pads.Length - 1].transform.position;
        deltaPositions.y = 0;
        player.transform.LookAt(player.transform.position + deltaPositions);

        ProcessSignal.Default.Send(new SignalPlayerSpawned { player = player });
        ProcessSignal.Default.Send(new SignalControlEnabled { value = true });
    }

    #region signals


    private void OnEnable()
    {
        ProcessSignal.Default.Add(this);
    }
    private void OnDisable()
    {
        ProcessSignal.Default.Remove(this);
    }


    public void HandleSignal(SignalHitPad arg)
    {
        if (arg.pad.Type == PadType.Destructable || arg.pad.Type == PadType.Persistant)
        {
            if (arg.dist <= GameSettings.instance.data.perfectDistance)

                SpawnMessage("Perfect");
            else if (arg.dist <= GameSettings.instance.data.goodDistance)
                SpawnMessage("Good");

            Score(arg.pad.PadNumber);
        }
        else if (arg.pad.Type == PadType.Finish)
        {
            Finish();
        }


    }

    private void Finish()
    {
        ProcessSignal.Default.Send(new SignalControlEnabled { value = false });
    }

    private void Score(int padNumber)
    {
        
    }

    public void HandleSignal(SignalGameover arg)
    {
        DeReset();
    }
    #endregion
}
