using System;
using UnityEngine;

public abstract class TetrominoStorer : MonoBehaviour
{

    private Tetris tetris;
    private Tetromino stored;

    public event Action<Tetromino> TetrominoChanged;


    //Circular dependecy now, gameStarted event should be moved out to a seperate class....
    private void Start()
    {
        tetris = FindObjectOfType<Tetris>();
        tetris.GameStarted += Clear;
    }

    protected Tetromino Stored
    {
        get { return stored; }
        set
        {
            stored = value;
            TetrominoChanged?.Invoke(value);
        }
    }

    protected virtual void Clear()
    {
        Stored = null;
    }
}
