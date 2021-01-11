using System;
using UnityEngine;

public abstract class TetrominoStorer : MonoBehaviour
{
    [SerializeField]
    protected TetrisState tetrisState;
    private Tetromino stored;

    public event Action<Tetromino, Tetromino> TetrominoChanged;

    protected virtual void OnEnable() => tetrisState.OnClear += Clear;

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

    protected void Clear() => Stored = null;

    protected virtual void OnDisable() => tetrisState.OnClear -= Clear;
}
