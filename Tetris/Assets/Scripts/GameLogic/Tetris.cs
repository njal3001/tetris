using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public Grid grid;
    public AudioManager audioManager;

    [SerializeField]
    private TetrominoGenerator tetrominoGenerator;
    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    [Header("Level Properties")]
    public float[] levelClockTime = new float[] { 48/61f, 43/61f, 38/61f, 33/61f, 28/61f, 23/61f, 18/61f, 13/61f, 8/61f, 6/61f, 5/61f, 5/61f, 5/61f, 4/61f, 4/61f, 4/61f, 3/61f, 3/61f, 3/61f,
                                                  2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 2/61f, 1/61f };
    public int startingLevel;
    private int level;

    [Header("Speed Properties")]
    public float maxSoftDropClockTime = 0.06f;
    public float softDropClockTimeMultiplier = 0.5f;
    private float normalClockTime;
    private float softDropClockTime;
    private float activeClockTime;
    private float currClockTime;

    public float startFastMoveTime = 0.2f;
    private float currStartFastMoveTime;
    public float fastMoveTime = 0.06f;
    private float currFastMoveTime;

    public float maxTetrominoLockDelay = 2;
    private float currMaxTetrominoLockDelay;
    public float minTetrominoLockDelay = 0.5f;
    private float currMinTetrominoLockDelay;
    private bool tetrominoLockDelayed;
    private bool tetrominoLockDelayFinished;

    private bool tetrominoSpawned;

    [Header("Score System")]
    public float[] scoreList = new float[4];
    private float score;
    private int totalRowsCleared;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI linesText;
    public TextMeshProUGUI levelText;

    private bool wasMoved;
    private float prevInputX;

    private Tetromino activeTetromino;

    private bool canHold;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandlePlayerInput();

        currClockTime += Time.deltaTime;

        if (tetrominoLockDelayed)
        {
            HandleTetrominoWait();
        }

        if (!tetrominoSpawned && !grid.ClearRowsEffectPlaying())
        {
            HandleSpawnNextTetromino();
        }
        

        if(currClockTime >= activeClockTime)
        {
            currClockTime = 0;

            if (tetrominoSpawned)
            {
                HandleMoveTetrominoDown();
            }
        }
    }

    private void HandleClearFullRows()
    {
        int rowsCleared = grid.ClearFullRows();
        if (rowsCleared > 0)
        {

            if (rowsCleared == 4)
            {
                audioManager.Play("tetrisRowClear");
            }
            else
            {
                audioManager.Play("rowClear");
            }

            score += scoreList[rowsCleared - 1];
            totalRowsCleared += rowsCleared;

            if (totalRowsCleared >= (level - startingLevel) * 10 + 10)
            {
                level += 1;
                if (level < levelClockTime.Length)
                {
                    normalClockTime = levelClockTime[level];
                    softDropClockTime = Mathf.Min(maxSoftDropClockTime, normalClockTime * softDropClockTimeMultiplier);
                }
            }

            UpdateText();
        }
        else
        {
            audioManager.Play("tetrominoLock");
        }

        tetrominoSpawned = false;
    }

    private void HandleSpawnTetromino(Tetromino tetromino)
    {
        currClockTime = 0;
        activeTetromino = tetromino;
        activeTetromino.Spawn(new Vector2(3, 0), grid);
        tetrominoSpawned = true;
    }

    private void HandleSpawnNextTetromino()
    {
        HandleSpawnTetromino(tetrominoGenerator.NextTetromino);

        canHold = true;
    }

    private void HandleMoveTetrominoDown()
    {
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
        audioManager.Play("tetrisSong");
        score = 0;
        level = startingLevel;
        totalRowsCleared = 0;
        UpdateText();
        grid.Clear();
        currClockTime = 0;
        normalClockTime = levelClockTime[Mathf.Min(level, levelClockTime.Length - 1)];
        activeClockTime = normalClockTime;
        softDropClockTime = Mathf.Min(maxSoftDropClockTime, normalClockTime * softDropClockTimeMultiplier);
        canHold = true;

        HandleSpawnNextTetromino();
    }

    private void UpdateText()
    {
        scoreText.text = score.ToString();
        linesText.text = totalRowsCleared.ToString();
        levelText.text = level.ToString();
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
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleHardDrop();
        }

        if (Input.GetKeyDown(KeyCode.C) && canHold)
        {
            HandleHoldTetromino();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            activeClockTime = softDropClockTime;
        }
        else
        {
            activeClockTime = normalClockTime;
        }

        bool moved = false;

        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX == prevInputX && inputX != 0)
        {
            currStartFastMoveTime += Time.deltaTime;
            currFastMoveTime += Time.deltaTime;
            if (currStartFastMoveTime >= startFastMoveTime)
            {
                if (currFastMoveTime >= fastMoveTime)
                {
                    if (activeTetromino.Move(inputX == 1 ? Vector2.right : Vector2.left))
                    {
                        moved = true;
                    }
                    currFastMoveTime = 0;
                }
            }
        }
        else
        {
            currStartFastMoveTime = 0;
            if (inputX != 0)
            {
                if (activeTetromino.Move(inputX == 1 ? Vector2.right : Vector2.left))
                {
                    moved = true;
                }
            }
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

    private void HandleHardDrop()
    {
        activeTetromino.HardDrop();
        HandleTetrominoFinished();
    }

}
