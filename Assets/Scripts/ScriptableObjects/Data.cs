using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Data")]
public class Data : ScriptableObject
{
    public static Data Default;
    public Data()
    {
        Default = this;
    }

    public float bottomPadJumpForce;
    public float percistantPadJumpForce;
    public float destructablePadJumpForce;
    public float playerMoveSpeed;
    public float playerRotate;
}
