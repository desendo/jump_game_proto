using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Data")]
public class Data : ScriptableObject
{
    public float bottomPadJumpForce;
    public float percistantPadJumpForce;
    public float destructablePadJumpForce;
    public float playerMoveSpeed;
    public float playerRotate;
    public float cameraZoomSpeed;
    public float perfectDistance;
    public float goodDistance;
}
