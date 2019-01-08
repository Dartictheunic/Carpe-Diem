using UnityEngine;
using System.Collections;

public class CurvedControls : MonoBehaviour
{
    public Vector2 Offset;
    public Material[] Mats;


    void Start()
    {
        foreach (Material M in Mats)
        {
            M.SetVector("_QOffset", Offset);
        }
    }
}