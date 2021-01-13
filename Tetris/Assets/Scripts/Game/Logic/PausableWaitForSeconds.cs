using UnityEngine;

public class PausableWaitForSeconds : CustomYieldInstruction
{
    private TetrisState tetrisState;
    private float time;
    private float currTime;

    public PausableWaitForSeconds(float time, TetrisState tetrisState)
    {
        this.time = time;
        this.tetrisState = tetrisState;
    }

    public override bool keepWaiting
    {
        get
        {
            if (tetrisState.IsPaused) return true;

            currTime += Time.deltaTime;
            return currTime < time;
        }
    }

}
