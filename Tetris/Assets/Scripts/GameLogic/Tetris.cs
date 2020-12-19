using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public Grid grid;
    public Grid nextTetrominoGrid;

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

    public float totalTetrominoWaitTime = 2;
    private float currTotalTetrominoWaitTime;
    public float tetrominoInBetweenWaitTime = 0.5f;
    private float currTetrominoInBetweenWaitTime;
    private bool tetrominoWaiting;
    private bool tetrominoWatitingFinished;

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

    private List<Tetromino> tetrominos;
    private List<Tetromino> nextTetrominos = new List<Tetromino>();
    private Tetromino activeTetromino;
    private Tetromino nextTetromino;

    private void Start()
    {
        Tetromino I = new BoxRotationTetromino(new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1) }, 4, new Block(Color.cyan));
        Tetromino J = new BlockRotationTetromino(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1) }, new Vector2(1, 1), new Block(Color.blue));
        Tetromino L = new BlockRotationTetromino(new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(2, 0) }, new Vector2(1, 1), new Block(new Color(1, 165/255, 0)));
        Tetromino O = new BoxRotationTetromino(new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) }, 2, new Block(Color.yellow));
        Tetromino S = new BlockRotationTetromino(new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(2, 0) }, new Vector2(1, 1), new Block(Color.green));
        Tetromino T = new BlockRotationTetromino(new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(2, 1) }, new Vector2(1, 1), new Block(Color.magenta));
        Tetromino Z = new BlockRotationTetromino(new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(2, 1) }, new Vector2(1, 1), new Block(Color.red));

        tetrominos = new List<Tetromino>{I, J, L, O, S, T, Z};
        //tetrominos = new List<Tetromino>{I, O};

        Initialize();
    }

    private void Update()
    {
        HandlePlayerInput();

        currClockTime += Time.deltaTime;

        if (tetrominoWaiting)
        {
            HandleTetrominoWait();
        }

        if (!tetrominoSpawned && !grid.ClearedRowsFading())
        {
            HandleSpawnTetromino();
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

        tetrominoSpawned = false;
    }

    private void HandleSpawnTetromino()
    {
        currClockTime = 0;
        activeTetromino = nextTetromino;
        UpdateNextTetromino();
        activeTetromino.Spawn(new Vector2(3, 0), grid);
        tetrominoSpawned = true;
    }

    private void UpdateNextTetromino()
    {
        nextTetromino = GetNextTetromino();
        nextTetrominoGrid.Clear();
        nextTetromino.Spawn(new Vector2(0, 0), nextTetrominoGrid);
    }

    private void HandleMoveTetrominoDown()
    {
        if (!activeTetromino.Move(Vector2.up) && tetrominoWatitingFinished)
        {
            HandleTetrominoFinished();
            tetrominoWatitingFinished = false;
        }
        else if (!activeTetromino.CanMove(Vector2.up) && !tetrominoWaiting)
        {
            tetrominoWaiting = true;
            wasMoved = false;
            currTotalTetrominoWaitTime = 0;
            currTetrominoInBetweenWaitTime = 0;
        }
    }

    private void HandleTetrominoWait()
    {
        currTotalTetrominoWaitTime += Time.deltaTime;
        currTetrominoInBetweenWaitTime += Time.deltaTime;

        bool waitOver = false;

        if (currTotalTetrominoWaitTime >= totalTetrominoWaitTime)
        {
            waitOver = true;
        }
        else if (currTetrominoInBetweenWaitTime >= tetrominoInBetweenWaitTime)
        {
            if (wasMoved)
            {
                currTetrominoInBetweenWaitTime = 0;
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
                tetrominoWatitingFinished = true;
            }
            tetrominoWaiting = false;
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
        print("GAME OVER \n Your score was: " + score);
        Initialize();
    }

    private void Initialize()
    {
        score = 0;
        level = startingLevel;
        totalRowsCleared = 0;
        UpdateText();
        grid.Clear();
        nextTetrominos.Clear();
        UpdateNextTetromino();
        currClockTime = 0;
        normalClockTime = levelClockTime[Mathf.Min(level, levelClockTime.Length - 1)];
        activeClockTime = normalClockTime;
        softDropClockTime = Mathf.Min(maxSoftDropClockTime, normalClockTime * softDropClockTimeMultiplier);

        HandleSpawnTetromino();
    }

    private void UpdateText()
    {
        scoreText.text = "SCORE: " + score;
        linesText.text = "LINES: " + totalRowsCleared;
        levelText.text = "LEVEL: " + level;
    }

    private Tetromino GetNextTetromino()
    {
        if (nextTetrominos.Count == 0)
        {
            nextTetrominos.AddRange(tetrominos);
        }

        Tetromino tetromino = nextTetrominos[Random.Range(0, nextTetrominos.Count)];
        nextTetrominos.Remove(tetromino);
        return tetromino;
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

        if (Input.GetKey(KeyCode.DownArrow))
        {
            activeClockTime = softDropClockTime;
        }
        else
        {
            activeClockTime = normalClockTime;
        }

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
                        wasMoved = true;
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
                    wasMoved = true;
                }
            }
        }

        prevInputX = inputX;

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (activeTetromino.Rotate(true)) 
            {
                wasMoved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (activeTetromino.Rotate(false))
            {
                wasMoved = true;
            }
        }
    }

    private void HandleHardDrop()
    {
        activeTetromino.HardDrop();
        HandleTetrominoFinished();
    }

}
