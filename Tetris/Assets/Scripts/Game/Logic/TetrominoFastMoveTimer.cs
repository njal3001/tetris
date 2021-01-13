using System.Collections;
using UnityEngine;

public class TetrominoFastMoveTimer : AbstractTimer
{
    [SerializeField]
    private TetrisState tetrisState;

    [SerializeField]
    private float fastMoveTime;
    [SerializeField]
    private float startFastMoveTime;

    private Coroutine currCoroutine;

    protected override float GetTime()
    {
        return fastMoveTime;
    }

    public override void StartTimer()
    {
        PreTimerStarted = true;
        currCoroutine = StartCoroutine(InvokeTimer());
    }

    private IEnumerator InvokeTimer()
    {
        yield return new PausableWaitForSeconds(startFastMoveTime, tetrisState);

        base.StartTimer();
    }

    public bool PreTimerStarted
    {
        get;
        private set;
    }

    public override void StopTimer()
    {
        base.StopTimer();
        StopCoroutine(currCoroutine);
        PreTimerStarted = false;
    }
}
