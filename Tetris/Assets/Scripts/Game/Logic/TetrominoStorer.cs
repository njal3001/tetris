using System;
using UnityEngine;

public abstract class TetrominoStorer : MonoBehaviour
{
    [SerializeField]
    private TetrisState tetris;
    private Tetromino stored;

    public event Action<Tetromino, Tetromino> TetrominoChanged;

    private void OnEnable() => tetris.GameStarted += Clear;

    protected Tetromino Stored
    {
        get => stored;
        set
        {
            Tetromino prev = stored;
            stored = value;
            TetrominoChanged?.Invoke(prev, value);
        }
    }

    protected virtual void Clear() => Stored = null;

    private void OnDisable() => tetris.GameStarted -= Clear;
}
