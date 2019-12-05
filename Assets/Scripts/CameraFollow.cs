using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
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

    private void Awake()
    {
        verticalDistance = targetHorizontalDistance;
        horizontalDistance = targetVerticalDistance;
    }
    public void SetCameraDistance(float addValue)
    {
        if (addValue == 0) return;
        targetVerticalDistance = Mathf.Clamp(targetVerticalDistance + addValue, verticalDistanceMin, verticalDistanceMax);
        targetHorizontalDistance = Mathf.Clamp(targetHorizontalDistance + addValue, horizontalDistanceMin, horizontalDistanceMax);
    }
    
    void Update()
    {
        if (targetTransform == null) return;

        horizontalDistance = Mathf.Lerp(horizontalDistance, targetHorizontalDistance, lerpFactor);
        verticalDistance = Mathf.Lerp(verticalDistance, targetVerticalDistance, lerpFactor);

        cameraPositionHorizontal = - targetTransform.forward * horizontalDistance;
        cameraPositionVertical = targetTransform.up * verticalDistance;

        goalCameraPosition = cameraPositionHorizontal + cameraPositionVertical + targetTransform.position;
        
        transform.position = goalCameraPosition;
        transform.LookAt(targetTransform, Vector3.up);

    }
    
    private void LateUpdate()
    {
        //transform.forward = new Vector3(transform.forward.x, targetTransform.forward.y, transform.forward.z);
    }
}