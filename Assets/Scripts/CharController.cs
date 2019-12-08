using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
[RequireComponent(typeof(Rigidbody))]
public class CharController : MonoBehaviour,IReceive<SignalFirstRank>
{

    public UnityEvent OnFirstRank;
    public UnityEvent OnNotFirstRank;
    Rigidbody rb;
    [SerializeField] Animator anim;
    public bool isPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    //float rotation;
    internal void MoveForward(float speed)
    {
        
            rb.MovePosition(rb.position + transform.forward * speed);

    }
    private void Update()
    {

        if (transform.position.y < -100f)
        {
            ProcessSignal.Default.Send(new SignalDie { charController = this }); ;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayer) return;
        CharController charController = collision.collider.gameObject.GetComponent<CharController>();
        if (charController != null)
        {
            if ((transform.position.y - collision.collider.transform.position.y) > 0)
            {
                charController.Kill();
                ProcessSignal.Default.Send(new SignalMurder { killer = this, victim = charController }); ;

            }
        }
    }

    internal string GetName()
    {
        if (isPlayer)
            return "You";
        else
            return "bot";
    }

    private void Kill()
    {
        var bot  = gameObject.GetComponent<CharController>();
        var col  = gameObject.GetComponent<Collider>();
        if(bot!=null)
            bot.enabled = false;
        if (col != null)
            col.enabled = false;

        

    }

    internal void Rotate(float angle)
    {
        transform.Rotate(Vector3.up, angle);
    }    
    public void AddImpulse(Vector3 impulse)
    {
       rb.AddForce(impulse, ForceMode.Impulse);
    }

    internal void OnEnterPadZone()
    {
        anim.SetTrigger("hit");
    }

    internal void HitByPad(PadType padType, Vector3 forceDir, Vector3 impulse)
    {
        

        if (padType == PadType.Persistant)
        {
            AddImpulse(GameSettings.instance.data.percistantPadJumpImpulse * forceDir);
        }
        else if (padType == PadType.Bottom) 
            AddImpulse(impulse);
        
        else if (padType == PadType.Destructable)
            AddImpulse(GameSettings.instance.data.destructablePadJumpImpulse * forceDir);

        else if (padType == PadType.Finish)
        {
            
            anim.SetLayerWeight(1, 0);
            anim.SetTrigger("victory");
        }
        

    }
    private void OnEnable()
    {
        ProcessSignal.Default.Add(this);
    }
    private void OnDisable()
    {
        ProcessSignal.Default.Remove(this);
    }
    public void HandleSignal(SignalFirstRank arg)
    {
        if (arg.charController == this)
            OnFirstRank.Invoke();
        else
            OnNotFirstRank.Invoke();
    }
}

