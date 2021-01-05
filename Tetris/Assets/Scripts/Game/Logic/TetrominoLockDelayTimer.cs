using System;
using UnityEngine;

public class TetrominoLockDelayTimer : MonoBehaviour
{

    [SerializeField]
    private SimpleTimer lockDelayTimer;
    [SerializeField]
    private SimpleTimer totalLockDelayTimer;

    public event Action Finished;

    private void OnEnable()
    {
        lockDelayTimer.Tick += OnTick;
        totalLockDelayTimer.Tick += OnTick;
    }

    public bool TimerOn()
    {
        return totalLockDelayTimer.TimerOn;
    }
        
    public void StartTimer()
    {
        lockDelayTimer.StartTimer();
        totalLockDelayTimer.StartTimer();
    }

    public void ResetLockDelay()
    {
        if (lockDelayTimer.TimerOn)
        {
            lockDelayTimer.StopTimer();
            lockDelayTimer.StartTimer();
        }
    }

    public void StopTimer()
    {
        lockDelayTimer.StopTimer();
        totalLockDelayTimer.StopTimer();
    }

    private void OnTick()
    {
        StopTimer();
        Finished?.Invoke();
    }

    private void OnDisable()
    {
        lockDelayTimer.Tick -= OnTick;
        totalLockDelayTimer.Tick -= OnTick;
    }
}
