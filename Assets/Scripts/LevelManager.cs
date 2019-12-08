using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LevelManager : MonoBehaviour,IReceive<SignalHitPad>, IReceive<SignalDie>, IReceive<SignalMurder>
{
    [SerializeField] CharController playerPrefab;
    [SerializeField] CharController botPrefab;
    [SerializeField] UIController uiController;
    [SerializeField] Transform playerSpawnPosition;
    [SerializeField] Transform[] botSpawnPositions;

    [SerializeField] Vector3 spawnOffset;
    Dictionary<CharController, int> charsScoreData;
    CharController player;
    CharController[] bots;
    private void Awake()
    {
        InitLevel();
    }

    private void Start()
    {
        StartLevel();
    }
    void StartLevel()
    {        
        SpawnPlayer();
        SpawnBot(0);
    }
    void ReStartLevel()
    {
        SpawnPlayer();
    }

    public void InitLevel()
    {
        charsScoreData = new Dictionary<CharController, int>();

    }

    private void ResetLevel()
    {
        charsScoreData = new Dictionary<CharController, int>();

        Pad[] pads = FindObjectsOfType<Pad>();
        for (int i = 0; i < pads.Length; i++)
        {
            pads[i].ResetPad();
        }        
    }

    public void SpawnPlayer()
    {
        if (player != null)
            Destroy(player.gameObject);

        player = Instantiate(playerPrefab);
        player.isPlayer = true;        

        player.transform.position = playerSpawnPosition.position +  spawnOffset;
        player.transform.rotation = playerSpawnPosition.rotation;        

        ProcessSignal.Default.Send(new SignalCharSpawned { charController = player });
        ProcessSignal.Default.Send(new SignalControlEnabled { value = true });
    }
    void SpawnBot(int i)
    {

        var bot = Instantiate(botPrefab);
        bot.isPlayer = false;

        bot.transform.position = botSpawnPositions[i].position + spawnOffset;
        bot.transform.rotation = botSpawnPositions[i].rotation;

        ProcessSignal.Default.Send(new SignalCharSpawned { charController = bot });
    }
    private void Finish()
    {
        ProcessSignal.Default.Send(new SignalControlEnabled { value = false });
    }
    int maxPad;
    int minPad;
    private void Score(int padNumber, CharController charController)
    {
        if (charsScoreData.ContainsKey(charController))
            charsScoreData[charController] = padNumber;
        else
            charsScoreData.Add(charController, padNumber);

        HandleScoreChange();
    }
    private void HandleScoreChange()
    {
        foreach (var item in charsScoreData)
        {
            

            if (item.Key.isPlayer)
            {
                minPad = item.Value;
                if (maxPad < minPad) maxPad = minPad;
                float p = 1f-((float)minPad / (float)maxPad);
                
                uiController.SetProgress(p);
            }
        }
        uiController.UpdateRank(charsScoreData);
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
            if (arg.charController.isPlayer && !arg.pad.AlreadyHit)
            {
                if (arg.dist <= GameSettings.instance.data.perfectDistance)
                    uiController.SpawnMessage("Perfect");
                else if (arg.dist <= GameSettings.instance.data.goodDistance)
                    uiController.SpawnMessage("Good");
            }
            Score(arg.pad.PadNumber, arg.charController);
        }
        else if (arg.pad.Type == PadType.Finish)
        {
            Score(arg.pad.PadNumber, arg.charController);
            Finish();
            uiController.UpdateRankMenu(charsScoreData);

        }
    }


    public void HandleSignal(SignalDie arg)
    {
        if (arg.charController.isPlayer)
        {
            ResetLevel();
            ReStartLevel();
        }
        else
        {

        }
    }

    public void HandleSignal(SignalMurder arg)
    {
        if(arg.killer.isPlayer)
            uiController.SpawnMessage("Kill!");

    }
    #endregion
}
