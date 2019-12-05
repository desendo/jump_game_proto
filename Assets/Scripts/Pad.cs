using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Collider))]
public class Pad : MonoBehaviour
{

    [SerializeField] PadType padType;
    Collider col;
    Vector3 forceDir;
    private void Awake()
    {
        col = GetComponent<Collider>();
        forceDir = Vector3.up;
    }
    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        var charController  = other.gameObject.GetComponent<CharacterController>();
        if (charController != null)
        {
            charController.OnEnterPadZone();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var charController = other.gameObject.GetComponent<CharacterController>();
        if (charController != null)
        {
            charController.OnExitPadZone();
        }
    }
    private void HandleCollision(Collision collision)
    {
        CharacterController charController = collision.collider.gameObject.GetComponent<CharacterController>();
        if (charController == null) return;
        if (charController.charType == CharType.Bot)
        {
            charController.AddForce(Data.Default.percistantPadJumpForce * forceDir);
        }
        else
        {

            charController.HitByPad(padType,forceDir);
            if (padType == PadType.Destructable)
            {
            
                Destruct();
            }
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

            rb.transform.parent = transform.parent.parent;

            rb.transform.DOScale(0.1f, 2).OnComplete( delegate { Destroy(rb.gameObject); });
        }
        col.enabled = false;
        //Destroy(gameObject);
    }

}

public enum PadType
{
    Bottom, Persistant, Destructable
}