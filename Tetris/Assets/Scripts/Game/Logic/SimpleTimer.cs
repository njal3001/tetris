using System;
using UnityEngine;

public class SimpleTimer : AbstractTimer
{
    [SerializeField]
    private float time;

    protected override float GetTime()
    {
        return time;
    }
}
