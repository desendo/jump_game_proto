using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
[RequireComponent(typeof(Collider))]
public class Pad : MonoBehaviour
{
    public UnityEvent OnHit;
    public UnityEvent OnHitFirstTime;

    [SerializeField] PadType padType;
    [SerializeField] int padNumber;
    [SerializeField] TMP_Text numberView;
    
    bool alreadyHit;
    Collider col;
    Vector3 forceDir;
    public PadType Type { get => padType; }
    public int PadNumber { get => padNumber; }

    MeshRenderer[] meshRenderers;
    Quaternion[] meshRenderersRotations;
    Vector3[] meshRenderersPositions;

    private void Awake()
    {
        col = GetComponent<Collider>();
        forceDir = Vector3.up;
        meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        meshRenderersRotations = new Quaternion[meshRenderers.Length];
        meshRenderersPositions = new Vector3[meshRenderers.Length];
    }
    
    void SavePositions()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderersPositions[i] =  meshRenderers[i].transform.position;
            meshRenderersRotations[i] = meshRenderers[i].transform.rotation;
        }
    }
    public void DoReset()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].transform.position = meshRenderersPositions[i];
            meshRenderers[i].transform.rotation = meshRenderersRotations[i];
        }
        col.enabled = true;
        alreadyHit = false;
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    public void InitNumber(int number)
    {        
        padNumber = number;
        if (numberView != null)
        {
            numberView.text = padNumber.ToString();
        }
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
    private void OnTriggerExit(Collider other)
    {
        var charController = other.gameObject.GetComponent<CharController>();
        if (charController != null)
        {
            charController.OnExitPadZone();
        }
    }

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

        if (!alreadyHit)
        {
            OnHitFirstTime.Invoke();
            ProcessSignal.Default.Send(new SignalHitPad {charController = charController, pad = this, dist = distanceToCenterOfPad });

            alreadyHit = true;
        }

        charController.HitByPad(padType, forceDir);

        if (charController.isPlayer && padType == PadType.Destructable)
        {
            Destruct();           
        }
        
            
    }
    public void Destruct()
    {
        var cells = transform.GetComponentsInChildren<Renderer>();
        foreach (var item in cells)
        {
            item.gameObject.AddComponent<SphereCollider>();

            var rb = item.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.mass = 0;

            rb.transform.SetParent ( transform.parent.parent);

            rb.transform.DOScale(0.1f, 2).OnComplete( delegate {
                Destroy(rb);
                gameObject.SetActive(false);
                });
        }
        col.enabled = false;
    }

}

public enum PadType
{
    Bottom, Persistant, Destructable, Finish
}