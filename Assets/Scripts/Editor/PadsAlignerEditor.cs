using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PadsAligner))]
public class PadsAlignerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PadsAligner myTarget = (PadsAligner)target;
        if (GUILayout.Button("Align And Assign Numbers"))
        {
            myTarget.AlignAndAssign();
        }
        if (GUILayout.Button("Draw lines"))
        {
            myTarget.DrawLines();
        }
    }
}
