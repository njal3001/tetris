using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoInput : MonoBehaviour
{

    [SerializeField]
    private TetrisState tetrisState;
    [SerializeField]
    private TetrominoSpawner spawner;

    [SerializeField]
    private TetrominoFall tetrominoFall;

    [SerializeField]
    private KeyCode moveRightKey;
    [SerializeField]
    private KeyCode moveLeftKey;
    [SerializeField]
    private TetrominoFastMoveTimer fastMoveTimer;

    [SerializeField]
    private KeyCode rightRotationKey;
    [SerializeField]
    private KeyCode leftRotationKey;

    [SerializeField]
    private KeyCode softDropKey;

    [SerializeField]
    private KeyCode hardDropKey;

    [SerializeField]
    private KeyCode holdKey;
    [SerializeField]
    private TetrominoHolder holder;
    private bool canHold = true;

    private Tetromino tetromino;

    public event Action TetrominoMoved;

    private void OnEnable()
    {
        spawner.TetrominoSpawned += OnTetrominoSpawned;
        fastMoveTimer.Tick += OnFastMoveTick;
        holder.TetrominoHeld += OnTetrominoHeld;
        tetrisState.OnTetrominoLocked += OnTetrominoLocked;
        tetrisState.OnClear += OnTetrominoLocked;
    }

    private void OnTetrominoSpawned(Tetromino tetromino)
    {
        this.tetromino = tetromino;
        tetromino.PosChanged += OnTetrominoPosChanged;
    }

    private void OnTetrominoPosChanged(Vector2[] newPos) => TetrominoMoved?.Invoke();

    private void OnTetrominoHeld(Tetromino prevT) => Reset();

    private void OnTetrominoLocked()
    {
        Reset();
        canHold = true;
    }

    private void Reset()
    {
        if (tetromino != null) tetromino.PosChanged -= OnTetrominoPosChanged;
        tetromino = null;
        StopFastMove();
    }

    private void Update()
    {
        if (tetromino == null) return;

        if (Input.GetKeyDown(holdKey) && canHold)
        {
            holder.Hold(tetromino);
            canHold = false;
            return;
        }

        if (Input.GetKeyDown(hardDropKey))
        {
            tetromino.HardDrop();
            tetrisState.TetrominoLocked(tetromino);
            return;
        }

        tetrominoFall.SoftDropOn(Input.GetKey(softDropKey));

        if (Input.GetKeyDown(rightRotationKey))
            tetromino.Rotate(true);
        else if (Input.GetKeyDown(leftRotationKey))
            tetromino.Rotate(false);

        HandleMoveInput();
    }

    private int prevMoveInput;
    private bool fastMoveTicked;

    private void HandleMoveInput()
    {
        int moveInput = 0;
        bool righMoveKeyDown = Input.GetKey(moveRightKey);
        bool leftMoveKeyDown = Input.GetKey(moveLeftKey);

        if (righMoveKeyDown && !leftMoveKeyDown) moveInput = 1;
        else if (!righMoveKeyDown && leftMoveKeyDown) moveInput = -1;

        if (moveInput == prevMoveInput && moveInput != 0)
        {
            if (!fastMoveTimer.PreTimerStarted)
                fastMoveTimer.StartTimer();

            if (fastMoveTicked)
            {
                MoveTetromino(moveInput);
                fastMoveTicked = false;
            }
        }
        else
        {
            StopFastMove();

            if (moveInput != 0)
                MoveTetromino(moveInput);
        }

        prevMoveInput = moveInput;
    }

    private void MoveTetromino(int moveInput) => tetromino.Move(new Vector2(moveInput, 0));

    private void OnFastMoveTick() => fastMoveTicked = true;

    private void StopFastMove()
    {
        if (fastMoveTimer.PreTimerStarted)
            fastMoveTimer.StopTimer();

        fastMoveTicked = false;
    }

    private void OnDisable()
    {
        spawner.TetrominoSpawned -= OnTetrominoSpawned;
        fastMoveTimer.Tick -= OnFastMoveTick;
        holder.TetrominoHeld -= OnTetrominoHeld;
        tetrisState.OnTetrominoLocked -= OnTetrominoLocked;
        tetrisState.OnClear -= OnTetrominoLocked;
    }

}
