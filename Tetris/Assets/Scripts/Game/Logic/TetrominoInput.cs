using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoInput : MonoBehaviour
{

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
    private TetrominoHolder tetrominoHolder;
    private bool canHold = true;

    private Tetromino tetromino;

    private void OnEnable()
    {
        spawner.TetrominoSpawned += OnTetrominoSpawned;
        tetrominoFall.TetrominoFinished += OnTetrominoFinished;
        fastMoveTimer.Tick += OnFastMoveTick;
    }

    private void OnTetrominoSpawned(Tetromino tetromino) => this.tetromino = tetromino;

    private void OnTetrominoFinished()
    {
        tetromino = null;
        canHold = true;
    }

    private void Update()
    {
        if (tetromino == null) return;

        if (Input.GetKeyDown(holdKey) && canHold)
        {
            tetrominoHolder.Hold(tetromino);
            canHold = false;
            return;
        }

        if (Input.GetKeyDown(hardDropKey))
        {
            tetrominoFall.HardDrop();
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
            fastMoveTimer.StopTimer();
            fastMoveTicked = false;

            if (moveInput != 0)
                MoveTetromino(moveInput);
        }

        prevMoveInput = moveInput;
    }

    private void MoveTetromino(int moveInput) => tetromino.Move(new Vector2(moveInput, 0));

    private void OnFastMoveTick()  => fastMoveTicked = true; 

    private void OnDisable()
    {
        spawner.TetrominoSpawned -= OnTetrominoSpawned;
        tetrominoFall.TetrominoFinished -= OnTetrominoFinished;
        fastMoveTimer.Tick -= OnFastMoveTick;
    }

}
