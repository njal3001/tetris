using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TetrominoDisplay))]
public class TetrominoDisplayEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying) return;

        TetrominoDisplay nextTetrominoDisplay = (TetrominoDisplay)target;

        nextTetrominoDisplay.CreateDisplay();
    }
}
