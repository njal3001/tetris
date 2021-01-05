using System;
using UnityEngine;

public abstract class AbstractTimer : MonoBehaviour
{

    private float currTime;

    public event Action Tick;

    public bool TimerOn
    {
        get;
        private set;
    }

    protected abstract float GetTime();
 

    public virtual void StartTimer()
    {
        TimerOn = true;
        currTime = 0;
    }

    public virtual void StopTimer()
    {
        TimerOn = false;
    }

    private void Update()
    {
        if (TimerOn)
        {
            if (currTime >= GetTime())
            {
                Tick?.Invoke();
                currTime = 0;
            }
            else
                currTime += Time.deltaTime;
        }
    }
}
