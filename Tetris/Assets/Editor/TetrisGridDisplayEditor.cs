using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TetrisGridDisplay))]
public class TetrisGridDisplayEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying) return;

        TetrisGridDisplay gridDisplay = (TetrisGridDisplay)target;

        gridDisplay.CreateDisplay();
    }
}
