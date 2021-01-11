using System;
using UnityEngine;

public class TetrisState : MonoBehaviour
{
    public event Action OnGameStarted;
    public event Action OnClear;
    public event Action OnTetrominoLocked;
    public event Action OnTetrominoLockedInBounds;
    public event Action OnGameOver;

    public void StartGame() => OnGameStarted?.Invoke();

    public void Clear() => OnClear?.Invoke();

    public void TetrominoLocked(Tetromino tetromino)
    {
        OnTetrominoLocked?.Invoke();

        if (tetromino.OutOfSight())
            OnGameOver?.Invoke();
        else
            OnTetrominoLockedInBounds?.Invoke();
    }

}
