using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharController : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] Animator anim;
    public bool isPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    internal void MoveForward(float speed)
    {
        
            rb.MovePosition(rb.position + transform.forward * speed);

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.y);
            float rotation = Mathf.Clamp( horizontalVelocity.magnitude, 0, 10f);
            anim.SetFloat("rotation", rotation / 10f);       

    }
    private void Update()
    {
        if (transform.position.y < -300f)
        {
            ProcessSignal.Default.Send(new SignalGameover { });
        }
    }
    internal void Rotate(float angle)
    {
        transform.Rotate(Vector3.up, angle);
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
            AddForce(GameSettings.instance.data.percistantPadJumpForce * forceDir);
        else if (padType == PadType.Bottom)
            AddForce(GameSettings.instance.data.bottomPadJumpForce * forceDir);
        else if (padType == PadType.Destructable)
            AddForce(GameSettings.instance.data.destructablePadJumpForce * forceDir);

        else if (padType == PadType.Finish)
        {
            anim.SetFloat("rotation",0);
            anim.SetLayerWeight(1, 0);
            anim.SetTrigger("victory");
        }
        

    }
}
