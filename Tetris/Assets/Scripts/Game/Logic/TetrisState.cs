using System;
using TMPro;
using UnityEngine;

public class TetrisState : MonoBehaviour
{
    /*
    [SerializeField]
    private TetrisGrid grid;
    */
    /*
    [SerializeField]
    private AudioManager audioManager;
    */

    /*
    //Should rather have this class handle game states, game started, game over....
    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;
    */

    public event Action GameStarted;
    public event Action TetrominoLocked;
    public event Action TetrominoLockedInBounds;
    public event Action GameOver;

    private void Start() => Initialize();

    private void Initialize()
    {
        GameStarted?.Invoke();

        //audioManager.Play("tetrisSong");
        //grid.Clear();
    }

    public void TetrominoIsLocked(Tetromino tetromino)
    {
        TetrominoLocked?.Invoke();

        if (tetromino.OutOfSight())
        {
            GameOver?.Invoke();
            HandleGameOver();
        }
        else
        {
            TetrominoLockedInBounds?.Invoke();
            //HandleClearFullRows();
        }
    }

    /*
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
    */

    private void HandleGameOver()
    {
        //audioManager.Play("gameOver");

        Initialize();
    }

}
