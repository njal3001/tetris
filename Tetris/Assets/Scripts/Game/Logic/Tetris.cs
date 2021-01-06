using System;
using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{

    [SerializeField]
    private TetrisGrid grid;
    [SerializeField]
    private AudioManager audioManager;

    //Should rather have this class handle game states, game started, game over and 
    [SerializeField]
    private TetrominoSpawner tetrominoSpawner;
    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;

    //TO be removed
    [SerializeField]
    private TetrominoFall tetrominoFall;

    public event Action GameStarted;

    private void OnEnable()
    {
        tetrominoFall.TetrominoFinished2 += OnTetrominoFinished;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        GameStarted?.Invoke();

        audioManager.Play("tetrisSong");
        grid.Clear();
        tetrominoSpawner.SpawnNextTetromino();
    }

    private void OnTetrominoFinished(Tetromino tetromino)
    {
        if (tetromino.OutOfSight())
            HandleGameOver();
        else
            HandleClearFullRows();
    }

    private void HandleClearFullRows()
    {
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

    private void HandleGameOver()
    {
        audioManager.Play("gameOver");

        Initialize();
    }

}
