using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadsAligner : MonoBehaviour
{
    // Start is called before the first frame update


    public void AlignAndAssign()
    {
        Pad[] pads = transform.GetComponentsInChildren<Pad>();
        if (pads.Length == 0) return;

        for (int i = 0; i < pads.Length; i++)
        {
            pads[i].transform.name = "pad " + (i + 1).ToString();
            if (i != 0)
            {
                pads[i].AlignNumber(pads[i - 1].transform);

            }
        }
    }

    public void DrawLines()
    {
        Pad[] pads = transform.GetComponentsInChildren<Pad>();
        LineRenderer[] lines = transform.GetComponentsInChildren<LineRenderer>();

        for (int i = 0; i < lines.Length; i++)
        {
            DestroyImmediate(lines[i].gameObject);
        }
        if (pads.Length == 0) return;

        for (int i = 0; i < pads.Length; i++)
        {
            
            if (i != 0)
            {

                for (int j = 0; j < pads.Length; j++)
                {
                    int new_i = 0;
                    int.TryParse(pads[j].name.Split(' ')[1], out new_i);
                    string pad_name = "pad " + (new_i + 1).ToString();

                    if (pads[i].name == pad_name)
                    {
                        DrawLine(pads[i].transform, pads[j].transform);
                    }
                }

            }
        }
    }
    private void DrawLine(Transform p1, Transform p2)
    {

        Debug.Log(p1.name + " to " + p2.name);
        GameObject line_gameobject = new GameObject("line");
        
        line_gameobject.transform.parent = transform;
        LineRenderer line = line_gameobject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Standard Unlit"));
        line.positionCount = 2;
        line.SetPosition(0, p1.position);
        line.SetPosition(1, p2.position);
        line.endWidth = 0.02f;
        line.startWidth = 0.02f;
    }
}
