using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TetrominoStorer : MonoBehaviour
{

    private Tetromino stored;

    public event Action<Tetromino> TetrominoChanged;

    protected Tetromino Stored
    {
        get { return stored; }
        set
        {
            stored = value;
            TetrominoChanged?.Invoke(value);
        }
    }
}
