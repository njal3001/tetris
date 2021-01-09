using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoGenerator : TetrominoStorer
{

    [SerializeField]
    private Tetromino[] tetrominos;

    private List<Tetromino> nextTetrominos = new List<Tetromino>();

    public event Action Cleared;

    public Tetromino NextTetromino
    {
        get
        {
            Tetromino oldNext = Stored;
            UpdateNextTetromino();
            return oldNext;
        }
    }

    private void UpdateNextTetromino()
    {
        if (nextTetrominos.Count == 0)
            nextTetrominos.AddRange(tetrominos);

        Stored = nextTetrominos[UnityEngine.Random.Range(0, nextTetrominos.Count)];
        nextTetrominos.Remove(Stored);
    }

    protected override void Clear()
    {
        nextTetrominos.Clear();
        base.Clear();
        UpdateNextTetromino();
        Cleared?.Invoke();
    }

}
