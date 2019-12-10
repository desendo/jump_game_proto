using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IReceive<SignalControlEnabled>, IReceive<SignalCharSpawned>
{

    [SerializeField] CharController characterMotor;
    [SerializeField] float distanceChangeSpeed;
    [Header("Projection Line Settings")]
    [SerializeField] LineRenderer projectionLine;
    [SerializeField] Vector3 projectionLineOffset;
    [SerializeField] Color colorProjectionHitted;
    [SerializeField] Color colorProjectionNotHitted;
    [SerializeField] LayerMask  whatIsProjectionTargets;
    [SerializeField] Transform projectionSpot;
    private Transform playerCharacterTransform;
    private bool controlsEnabled;

    private void Init(CharController characterController)
    {
        characterMotor = characterController;
        playerCharacterTransform = characterMotor.transform;
    }
    void Update()
    {
        if (characterMotor == null) return;

        HandleInput();

        UpdateProjection();

    }
    float rotateControlSpeed;
    float rotateControlSpeedAccels = 5f;
    private void HandleInput()
    {
        if (!controlsEnabled) return;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            rotateControlSpeed += Time.deltaTime * rotateControlSpeedAccels;
            if (Input.GetKey(KeyCode.D))
            {
                characterMotor.Rotate(GameSettings.instance.data.playerRotate * 60f * Time.deltaTime * rotateControlSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                characterMotor.Rotate(-GameSettings.instance.data.playerRotate * 60f * Time.deltaTime * rotateControlSpeed);
            }
        }
        else
            rotateControlSpeed -= Time.deltaTime * rotateControlSpeedAccels;

        if (rotateControlSpeed > 1) rotateControlSpeed = 1;
        if (rotateControlSpeed < 0) rotateControlSpeed = 0;

        if (Input.GetKey(KeyCode.W))
        {
            characterMotor.MoveForward(GameSettings.instance.data.playerMoveSpeed * 60f * Time.deltaTime);

            
        }
    }

    private void UpdateProjection()
    {
        if (projectionLine == null) return;
        projectionLine.SetPosition(0, playerCharacterTransform.position + projectionLineOffset);        
        

        Ray ray = new Ray(playerCharacterTransform.position, Vector3.down);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f, whatIsProjectionTargets))
        {

            projectionLine.SetPosition(1, raycastHit.point);
            projectionSpot.position = raycastHit.point;
            projectionLine.material.color = colorProjectionHitted;

        }
        else
        {
            projectionLine.SetPosition(1, playerCharacterTransform.up * -10000f);
            projectionSpot.position = playerCharacterTransform.up * -10000f;
            projectionLine.material.color = colorProjectionNotHitted;
        }
    }

    #region signals
    public void HandleSignal(SignalControlEnabled arg)
    {
        controlsEnabled = arg.value;
    }

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
            Init(arg.charController);
        }
    }
    #endregion
}
