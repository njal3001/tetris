using System;
using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    [SerializeField]
    private TetrisGrid grid;
    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;
    [SerializeField]
    private TetrominoFallTimer fallTimer;
    [SerializeField]
    private TetrominoFastMoveTimer fastMoveTimer;

    public AudioManager audioManager;

    [SerializeField]
    private TetrominoGenerator tetrominoGenerator;
    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    public float maxTetrominoLockDelay = 2;
    private float currMaxTetrominoLockDelay;
    public float minTetrominoLockDelay = 0.5f;
    private float currMinTetrominoLockDelay;
    private bool tetrominoLockDelayed;
    private bool tetrominoLockDelayFinished;

    private bool tetrominoSpawned;

    private bool wasMoved;
    private float prevInputX;

    private Tetromino activeTetromino;

    private bool canHold;
    private bool fastMoveTicked;

    public event Action GameStarted;

    private void OnEnable()
    {
        gridRowClearer.FullRowsClearingEnded += HandleSpawnNextTetromino;
        fallTimer.Tick += HandleMoveTetrominoDown;
        fastMoveTimer.Tick += OnFastMoveTick;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandlePlayerInput();

        if (tetrominoLockDelayed)
        {
            HandleTetrominoWait();
        }
    }

    private void HandleClearFullRows()
    {
        tetrominoSpawned = false;
        int lines = gridRowClearer.ClearFullRows();
        if (lines > 0)
        {

            if (lines == 4)
            {
                audioManager.Play("tetrisRowClear");
            }
            else
            {
                audioManager.Play("rowClear");
            }
        }
        else
        {
            audioManager.Play("tetrominoLock");
        }
    }

    private void HandleSpawnTetromino(Tetromino tetromino)
    {
        activeTetromino = tetromino;
        activeTetromino.Spawn();
        tetrominoSpawned = true;
        fallTimer.StartTimer();
    }

    private void HandleSpawnNextTetromino()
    {
        HandleSpawnTetromino(tetrominoGenerator.NextTetromino);

        canHold = true;
    }

    private void HandleMoveTetrominoDown()
    {
        if (!tetrominoSpawned) return;

        bool movedDown = activeTetromino.Move(Vector2.up);

        if (movedDown) audioManager.Play("tetrominoMove");

        if (!movedDown && tetrominoLockDelayFinished)
        {
            HandleTetrominoFinished();
            tetrominoLockDelayFinished = false;
        }
        else if (!activeTetromino.CanMove(Vector2.up) && !tetrominoLockDelayed)
        {
            tetrominoLockDelayed = true;
            wasMoved = false;
            currMaxTetrominoLockDelay = 0;
            currMinTetrominoLockDelay = 0;
        }
    }

    private void HandleTetrominoWait()
    {
        currMaxTetrominoLockDelay += Time.deltaTime;
        currMinTetrominoLockDelay += Time.deltaTime;

        bool waitOver = false;

        if (currMaxTetrominoLockDelay >= maxTetrominoLockDelay)
        {
            waitOver = true;
        }
        else if (currMinTetrominoLockDelay >= minTetrominoLockDelay)
        {
            if (wasMoved)
            {
                currMinTetrominoLockDelay = 0;
                wasMoved = false;
            }
            else
            {
                waitOver = true;
            }
        }

        if (waitOver)
        {
            if (!activeTetromino.CanMove(Vector2.up))
            {
                HandleTetrominoFinished();
            }
            else
            {
                tetrominoLockDelayFinished = true;
            }
            tetrominoLockDelayed = false;
        }
    }

    private void HandleTetrominoFinished()
    {
        fallTimer.StartTimer();

        if (activeTetromino.OutOfSight())
        {
            HandleGameOver();
        }
        else
        {
            HandleClearFullRows();
        }
    }

    private void HandleGameOver()
    {
        audioManager.Play("gameOver");

        Initialize();
    }

    private void Initialize()
    {
        GameStarted?.Invoke();

        audioManager.Play("tetrisSong");
        grid.Clear();
        canHold = true;

        HandleSpawnNextTetromino();
    }
    private void HandleHoldTetromino()
    {

        Tetromino currHolding = tetrominoHolder.Swap(activeTetromino);

        if (currHolding != null)
        {
            HandleSpawnTetromino(currHolding);
        }
        else
        {
            HandleSpawnNextTetromino();
        }

        canHold = false;

        audioManager.Play("tetrominoHold");
    }

    private void HandlePlayerInput()
    {
        if (!tetrominoSpawned)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            HandleHardDrop();

        if (Input.GetKeyDown(KeyCode.C) && canHold)
            HandleHoldTetromino();

        if (Input.GetKey(KeyCode.DownArrow))
            fallTimer.FastTimeOn = true;
        else
            fallTimer.FastTimeOn = false;

        bool moved = false;

        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX == prevInputX && inputX != 0)
        {
            if (!fastMoveTimer.PreTimerStarted)
                fastMoveTimer.StartTimer();

            if (fastMoveTicked)
            {
                fastMoveTicked = false;
                if (activeTetromino.Move(inputX == 1 ? Vector2.right : Vector2.left))
                    moved = true;
            }

        }
        else
        {
            fastMoveTimer.StopTimer();
            if (inputX != 0)
                if (activeTetromino.Move(inputX == 1 ? Vector2.right : Vector2.left))
                    moved = true;
        }

        prevInputX = inputX;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (activeTetromino.Rotate(true)) 
            {
                moved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (activeTetromino.Rotate(false))
            {
                moved = true;
            }
        }

        if (moved)
        {
            wasMoved = true;
            audioManager.Play("tetrominoMove");
        }
    }

    private void OnFastMoveTick()
    {
        fastMoveTicked = true;
    }

    private void HandleHardDrop()
    {
        activeTetromino.HardDrop();
        HandleTetrominoFinished();
    }

}
