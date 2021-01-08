using System;
using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{

    [SerializeField]
    private TetrisGrid grid;
    [SerializeField]
    private AudioManager audioManager;

    //Should rather have this class handle game states, game started, game over....
    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;


    public event Action GameStarted;
    public event Action TetrominoFinished;

    private void Start() => Initialize();

    private void Initialize()
    {
        GameStarted?.Invoke();

        audioManager.Play("tetrisSong");
        grid.Clear();
    }

    public void TetrominoIsFinished(Tetromino tetromino)
    {
        TetrominoFinished?.Invoke();

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
