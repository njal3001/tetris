using System;
using UnityEngine;

public class TetrominoFall : MonoBehaviour
{

    private Tetromino tetromino;

    [SerializeField]
    private TetrominoSpawner spawner;

    [SerializeField]
    private TetrominoFallTimer fallTimer;

    [SerializeField]
    private TetrominoLockDelayTimer lockDelayTimer;
    private bool lockDelayFinished;
    [SerializeField]
    private int maxLockDelayRestarts;
    private int lockDelayRestartCounter;

    public event Action TetrominoFinished;

    //To be removed
    public event Action<Tetromino> TetrominoFinished2;


    private void OnEnable() 
    {
        spawner.TetrominoSpawned += OnTetrominoSpawned;
        fallTimer.Tick += OnFallTimerTick;
        lockDelayTimer.Finished += OnLockDelayFinished;
        TetrominoFinished += OnTetrominoFinished;
    }

    private void OnTetrominoSpawned(Tetromino tetromino)
    {
        this.tetromino = tetromino;
        tetromino.PosChanged += OnTetrominoMoved;
        fallTimer.StartTimer();
    }

    private void OnTetrominoMoved(Vector2[] newPos) => lockDelayTimer.ResetLockDelay();

    private void OnLockDelayFinished() => lockDelayFinished = true;

    public void SoftDropOn(bool on) => fallTimer.FastTimeOn = on;

    public void HardDrop()
    {
        tetromino.HardDrop();
        TetrominoFinished?.Invoke();
    }

    private void OnFallTimerTick()
    {
        if (tetromino.Move(Vector2.up))
        {
            if (lockDelayTimer.TimerOn() && lockDelayRestartCounter <= maxLockDelayRestarts)
            {
                lockDelayRestartCounter++;
                lockDelayFinished = false;
                lockDelayTimer.StopTimer();
            }
        }
        else if (lockDelayFinished)
            TetrominoFinished?.Invoke();

        if (!tetromino.CanMove(Vector2.up) && !lockDelayTimer.TimerOn())
            lockDelayTimer.StartTimer();
    }

    private void OnTetrominoFinished()
    {
        tetromino.PosChanged -= OnTetrominoMoved;
        fallTimer.StopTimer();
        lockDelayFinished = false;
        lockDelayRestartCounter = 0;

        TetrominoFinished2?.Invoke(tetromino);
    }

    private void OnDisable()
    {
        spawner.TetrominoSpawned -= OnTetrominoSpawned;
        fallTimer.Tick -= OnFallTimerTick;
        lockDelayTimer.Finished -= OnLockDelayFinished;
        TetrominoFinished -= OnTetrominoFinished;
    }
}
