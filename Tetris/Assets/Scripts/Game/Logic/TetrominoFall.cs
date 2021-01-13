using System;
using UnityEngine;

public class TetrominoFall : MonoBehaviour
{

    private Tetromino tetromino;

    [SerializeField]
    private TetrisState tetris;

    [SerializeField]
    private TetrominoSpawner spawner;

    [SerializeField]
    private TetrominoHolder holder;

    [SerializeField]
    private TetrominoFallTimer fallTimer;

    [SerializeField]
    private TetrominoLockDelayTimer lockDelayTimer;
    private bool lockDelayFinished;
    [SerializeField]
    private int maxLockDelayRestarts;

    //Might do this differently
    private int lockDelayRestartCounter;


    private void OnEnable() 
    {
        spawner.TetrominoSpawned += OnTetrominoSpawned;

        fallTimer.Tick += OnFallTimerTick;
        lockDelayTimer.Finished += OnLockDelayFinished;

        holder.TetrominoHeld += OnTetrominoHeld;
        tetris.OnTetrominoLocked += Reset;
        tetris.OnClear += Reset;
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
            tetris.TetrominoLocked(tetromino);

        if (!tetromino.CanMove(Vector2.up) && !lockDelayTimer.TimerOn())
            lockDelayTimer.StartTimer();
    }

    private void OnTetrominoHeld(Tetromino prevT) => Reset();

    private void Reset()
    {
        if (tetromino != null) tetromino.PosChanged -= OnTetrominoMoved;
        fallTimer.StopTimer();
        lockDelayFinished = false;
        lockDelayRestartCounter = 0;
    }

    private void OnDisable()
    {
        spawner.TetrominoSpawned -= OnTetrominoSpawned;

        fallTimer.Tick -= OnFallTimerTick;
        lockDelayTimer.Finished -= OnLockDelayFinished;

        holder.TetrominoHeld -= OnTetrominoHeld;
        tetris.OnTetrominoLocked -= Reset;
        tetris.OnClear += Reset;
    }
}
