using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using System;

[RequireComponent(typeof(Collider))]
public class Pad : MonoBehaviour
{
    public UnityEvent OnHit;
    public UnityEvent OnHitFirstTime;
    public UnityEvent OnReset;

    [SerializeField] PadType padType;
    [SerializeField] int padNumber;
    [SerializeField] TMP_Text numberView;
    
    bool alreadyHit;
    Collider col;
    Vector3 forceDir;
    public PadType Type { get => padType; }
    public bool AlreadyHit { get => alreadyHit; }
    public int PadNumber { get => padNumber;  }

    MeshRenderer[] meshRenderers;
    Quaternion[] meshRenderersRotations;
    Vector3[] meshRenderersPositions;
    Vector3[] meshRenderersScales;

    private void Awake()
    {
        if (padType == PadType.Destructable || padType == PadType.Persistant)
        {
            int.TryParse(transform.name.Split(' ')[1], out padNumber);

            numberView.text = transform.name.Split(' ')[1];
        }
        col = GetComponent<Collider>();
        forceDir = transform.up;
        
        meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        meshRenderersRotations = new Quaternion[meshRenderers.Length];
        meshRenderersPositions = new Vector3[meshRenderers.Length];
        meshRenderersScales = new Vector3[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderersPositions[i] = meshRenderers[i].transform.position;
            meshRenderersRotations[i] = meshRenderers[i].transform.rotation;
            meshRenderersScales[i] = meshRenderers[i].transform.localScale;
        }
    }

    internal void AlignNumber(Transform t)
    {
        Transform number = transform.Find("hint_container");
        if (number == null) return;
        number.LookAt(t);
        Vector3 euler = number.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        number.eulerAngles = euler;
    }
    /// <summary>
    /// reseting pad condition whithout re-inst
    /// </summary>
    public void ResetPad()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].transform.position = meshRenderersPositions[i];
            meshRenderers[i].transform.rotation = meshRenderersRotations[i];
            meshRenderers[i].transform.localScale = meshRenderersScales[i];
        }
        col.enabled = true;
        alreadyHit = false;
        OnReset.Invoke();

    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        var charController  = other.gameObject.GetComponent<CharController>();
        if (charController != null)
        {
            charController.OnEnterPadZone();
        }
    }
    /// <summary>
    /// helper method to detect accuracy of jump
    /// </summary>
    /// <param name="collision"></param>
    /// <returns>distance from center</returns>
    float CalcHorizontalDistance(Collision collision)
    {
        Vector3 v1= collision.collider.transform.position;
        v1.y = 0;
        Vector3 v2 = transform.position;
        v2.y = 0;

        return (v1 - v2).magnitude;
    }
    private void HandleCollision(Collision collision)
    {
        CharController charController = collision.collider.gameObject.GetComponent<CharController>();
        if (charController == null) return;
        
        OnHit.Invoke();

        float distanceToCenterOfPad = CalcHorizontalDistance(collision);
        ProcessSignal.Default.Send(new SignalHitPad { charController = charController, pad = this, dist = distanceToCenterOfPad });
        
        charController.HitByPad(padType, forceDir, collision.impulse);

        if (charController.isPlayer )
        {
            OnHit.Invoke();
            if (!alreadyHit)
            {
                OnHitFirstTime.Invoke();
                alreadyHit = true;
            }
            if (padType == PadType.Destructable)
                Destruct();
        }       
            
    }

    /// <summary>
    /// decomposition of a destructable pad
    /// </summary>
    public void Destruct()
    {
        var cells = transform.GetComponentsInChildren<Renderer>();
        foreach (var item in cells)
        {
            Collider col = item.gameObject.AddComponent<SphereCollider>();

            var rb = item.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.mass = 0;


            rb.transform.DOScale(0.1f, 2).OnComplete( delegate {
                Destroy(rb);
                Destroy(col);
                });
        }
        col.enabled = false;
    }
}

public enum PadType
{
    Bottom, Persistant, Destructable, Finish
}