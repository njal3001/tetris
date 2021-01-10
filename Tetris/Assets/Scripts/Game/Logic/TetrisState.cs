using System;
using TMPro;
using UnityEngine;

public class TetrisState : MonoBehaviour
{
    public event Action GameStarted;
    public event Action TetrominoLocked;
    public event Action TetrominoLockedInBounds;
    public event Action GameOver;

    public void StartGame() => GameStarted?.Invoke();

    public void TetrominoIsLocked(Tetromino tetromino)
    {
        TetrominoLocked?.Invoke();

        if (tetromino.OutOfSight())
        {
            GameOver?.Invoke();
            HandleGameOver();
        }
        else
            TetrominoLockedInBounds?.Invoke();
    }

    private void HandleGameOver() => StartGame();

}
