using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisStats : MonoBehaviour
{
    
    //Element i : Base points for clearing i + 1 rows, for example scoreList[0] is amount of base points for clearing 1 row
    //Actual amount of points given has a level multiplier
    [SerializeField]
    private float[] scoreList = new float[4];

    [SerializeField]
    private int startingLevel;
    private int level;
    public event Action<int> LevelChanged;

    private float score;
    public event Action<float> ScoreChanged;

    private int lines;
    public event Action<int> LinesChanged;

    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;
    [SerializeField]
    private Tetris tetris;

    private int Level
    {
        set
        {
            level = value;
            LevelChanged?.Invoke(level);
        }
        get { return level; }
    }

    private float Score
    {
        set
        {
            score = value;
            ScoreChanged?.Invoke(score);
        }
        get { return score; }
    }

    private int Lines
    {
        set
        {
            lines = value;
            LinesChanged?.Invoke(lines);
        }
        get { return lines; }
    }

    private void OnEnable()
    {
        tetris.GameStarted += Reset;
        gridRowClearer.RowsCleared += OnRowsCleared;
    }

    private void Reset()
    {
        Level = startingLevel;
        Score = 0;
        Lines = 0;
    }


    //TODO: Should probably not trigger event if no lines where cleared....
    private void OnRowsCleared(int lines)
    {
        if (lines <= 0) return;

        Lines += lines;

        Score += scoreList[Mathf.Min(lines - 1, 3)] * (level + 1);

        if (Lines >= (Level - startingLevel) * 10 + 10)
        {
            Level += 1;
        }
    }

    private void OnDisable()
    {
        tetris.GameStarted -= Reset;
        gridRowClearer.RowsCleared -= OnRowsCleared;
    }
}
