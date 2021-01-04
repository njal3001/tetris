using System;
using UnityEngine;

public class TetrominoFallTimer : TetrominoTimer
{
    [SerializeField]
    private TetrisStats stats;

    [SerializeField]
    private float[] levelFallTime = new float[30];
    [SerializeField]
    private float fastDropSpeedMultiplier;
    [SerializeField]
    private float maxFastDropFallTime;

    private float normalTime;
    private float fastTime;


    private void OnEnable()
    {
        stats.LevelChanged += UpdateFallTime;
    }

    public bool FastTimeOn
    {
        private get;
        set;
    }

    private void UpdateFallTime(int level)
    {
        normalTime = levelFallTime[Mathf.Clamp(level, 0, 29)];
        fastTime = Mathf.Min(normalTime * (1 / fastDropSpeedMultiplier), maxFastDropFallTime);
    }

    protected override float GetTime()
    {
        return FastTimeOn ? fastTime : normalTime;
    }

    private void OnDisable()
    {
        stats.LevelChanged -= UpdateFallTime;
    }

}
