using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharController))]
public class BotAI : MonoBehaviour, IReceive<SignalHitPad>
{
    Pad[] pads;
    CharController charController;
    int lastHittedPad;
    Transform target;
    public void HandleSignal(SignalHitPad arg)
    {

        if (arg.charController != charController) return;
        target = null;

        lastHittedPad = arg.pad.PadNumber;
        List<Pad> pads = FindSameNumberPads(lastHittedPad - 1);
        if (pads.Count == 0) return;

        float deltaMagnitude = float.PositiveInfinity;
        foreach (var pad in pads)
        {
            float currentDelta = (pad.transform.position - transform.position).sqrMagnitude;
            if (currentDelta < deltaMagnitude && arg.pad.transform!= pad.transform)
            {
                target = pad.transform;
                deltaMagnitude = currentDelta;
            }
        }
    }

    const float epsilon = 0.05f;
    void Update()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;
        targetPosition.y = 0;
        Vector3 selfPosition = transform.position;
        selfPosition.y = 0;
        float distHorizontal = (targetPosition - selfPosition).sqrMagnitude;


        if (distHorizontal > 0.05f)
        {
            charController.MoveForward(GameSettings.instance.data.playerMoveSpeed * 60f * Time.deltaTime);
        }

        transform.LookAt(target);
        Vector3 rot = transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = rot;

    }
    List<Pad> FindSameNumberPads(int number)
    {
        List<Pad> sameNumberPads = new List<Pad>();

        for (int i = 0; i < pads.Length; i++)
        {
            if (pads[i].PadNumber == number)
                sameNumberPads.Add(pads[i]);
        }
        return sameNumberPads;
    }
    private void OnEnable()
    {
        ProcessSignal.Default.Add(this);
    }
    private void OnDisable()
    {
        ProcessSignal.Default.Remove(this);
    }
    void Start()
    {
        lastHittedPad = int.MaxValue;
        charController = GetComponent<CharController>();
        pads = FindObjectsOfType<Pad>();

    }



}
