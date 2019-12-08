using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour, IReceive<SignalCharSpawned>, IReceive<SignalHitPad>
{
    [Range(0f,1f)]
    [SerializeField] float lerpFactor = 0.1f;
    [SerializeField] Transform targetTransform;
    

    
    [SerializeField] private float targetHorizontalDistance = 1f;    
    [SerializeField] private float horizontalDistanceMin = 1f;


    [SerializeField] private float horizontalDistanceMax = 1f;    
    [SerializeField] private float targetVerticalDistance = 1f;
    [SerializeField] private float verticalDistanceMin = 1f;
    [SerializeField] private float verticalDistanceMax = 1f;
    float verticalDistance;
    float horizontalDistance;

    Vector3 cameraPositionHorizontal;
    Vector3 cameraPositionVertical;
    Vector3 goalCameraPosition;

    Camera cam;
    private void Awake()
    {
        verticalDistance = targetHorizontalDistance;
        horizontalDistance = targetVerticalDistance;
        cam = transform.GetComponentInChildren<Camera>();
    }
    public void SetCameraDistance(float addValue)
    {
        if (addValue == 0) return;
        Debug.Log(addValue);
        targetVerticalDistance = Mathf.Clamp(targetVerticalDistance + addValue, verticalDistanceMin, verticalDistanceMax);
        targetHorizontalDistance = Mathf.Clamp(targetHorizontalDistance + addValue, horizontalDistanceMin, horizontalDistanceMax);
    }
    
    void Update()
    {
        if (targetTransform == null) return;

        SetCameraDistance(Input.mouseScrollDelta.y * GameSettings.instance.data.cameraZoomSpeed);

        horizontalDistance = Mathf.Lerp(horizontalDistance, targetHorizontalDistance, lerpFactor);
        verticalDistance = Mathf.Lerp(verticalDistance, targetVerticalDistance, lerpFactor);

        cameraPositionHorizontal = - targetTransform.forward * horizontalDistance;
        cameraPositionVertical = targetTransform.up * verticalDistance;

        goalCameraPosition = cameraPositionHorizontal + cameraPositionVertical + targetTransform.position;
        
        transform.position = goalCameraPosition;
        transform.LookAt(targetTransform, Vector3.up);

    }
    void Shake(float strength)
    {
        cam.transform.DOShakePosition(0.3f, strength,30);
    }


    private void LateUpdate()
    {
        //transform.forward = new Vector3(transform.forward.x, targetTransform.forward.y, transform.forward.z);
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

    public void HandleSignal(SignalCharSpawned arg)
    {
        if (arg.charController.isPlayer)
        {
            targetTransform = arg.charController.transform;
        }
    }
    public void HandleSignal(SignalHitPad arg)
    {
        if (!arg.charController.isPlayer) return;
        if (arg.pad.Type == PadType.Destructable)
            Shake(1);
        else if (arg.pad.Type == PadType.Persistant)
            Shake(0.2f);
    }
    #endregion
}