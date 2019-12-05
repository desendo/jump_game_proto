using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] CharacterMotor characterMotor;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] float distanceChangeSpeed;
    [Header("Projection Line Settings")]
    [SerializeField] LineRenderer projectionLine;
    [SerializeField] Vector3 projectionLineOffset;
    [SerializeField] LayerMask  whatIsProjectionLineTargets;
    [SerializeField] Transform projectionSpot;
    private Transform playerCharacterTransform;
    private void Awake()
    {
        playerCharacterTransform = characterMotor.transform;
    }
    void Update()
    {
        cameraFollow.SetCameraDistance(Input.mouseScrollDelta.y * distanceChangeSpeed);
        
        if (Input.GetKey(KeyCode.W))
        {
            characterMotor.MoveForward();
        }

        if (Input.GetKey(KeyCode.A))
        {
            characterMotor.RotateLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            characterMotor.RotateRight();
        }

        UpdateProjection();

    }

    private void UpdateProjection()
    {
        if (projectionLine == null) return;
        projectionLine.SetPosition(0, playerCharacterTransform.position + projectionLineOffset);
        projectionLine.SetPosition(1, playerCharacterTransform.position + projectionLineOffset);
        

        Ray ray = new Ray(playerCharacterTransform.position, Vector3.down);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f, whatIsProjectionLineTargets))
        {
            
            projectionLine.SetPosition(1, raycastHit.point);
            projectionSpot.position = raycastHit.point;
        }
        else
            projectionSpot.position = playerCharacterTransform.up * -100f;
    }
}
