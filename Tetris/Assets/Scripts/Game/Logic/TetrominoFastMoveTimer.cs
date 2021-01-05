using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoFastMoveTimer : AbstractTimer
{

    [SerializeField]
    private float fastMoveTime;
    [SerializeField]
    private float startFastMoveTime;

    protected override float GetTime()
    {
        return fastMoveTime;
    }

    public override void StartTimer()
    {
        StartCoroutine(InvokeTimer());
        PreTimerStarted = true;
    }

    private IEnumerator InvokeTimer()
    {
        yield return new WaitForSeconds(startFastMoveTime);

        base.StartTimer();
    }

    public bool PreTimerStarted
    {
        get;
        private set;
    }

    public override void StopTimer()
    {
        StopCoroutine(InvokeTimer());
        PreTimerStarted = false;
        base.StopTimer();
    }
}
