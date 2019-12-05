using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{

    Rigidbody rb;
    public CharType charType;
    [SerializeField] Animator anim;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    internal void MoveForward()
    {
        if (charType == CharType.Player)
        {
            rb.MovePosition(rb.position + transform.forward * Data.Default.playerMoveSpeed);
        }

    }

    internal void RotateRight()
    {

    }

    internal void RotateLeft()
    {

    }
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    internal void OnEnterPadZone()
    {
        anim.SetTrigger("hit");
    }

    internal void OnExitPadZone()
    {
        
    }

    internal void HitByPad(PadType padType, Vector3 forceDir)
    {
        
        if (padType == PadType.Persistant)
            AddForce(Data.Default.percistantPadJumpForce * forceDir);
        else if (padType == PadType.Bottom)
            AddForce(Data.Default.bottomPadJumpForce * forceDir);
        else if (padType == PadType.Destructable)
            AddForce(Data.Default.destructablePadJumpForce * forceDir);

    }
}
public enum CharType
{
    Bot,Player
}