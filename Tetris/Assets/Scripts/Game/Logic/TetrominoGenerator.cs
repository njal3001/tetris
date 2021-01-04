using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoGenerator : TetrominoStorer
{

    [SerializeField]
    private Tetromino[] tetrominos;

    private List<Tetromino> nextTetrominos = new List<Tetromino>();

    private void Start()
    {
        UpdateNextTetromino();
    }

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
        {
            nextTetrominos.AddRange(tetrominos);
        }

        Stored = nextTetrominos[Random.Range(0, nextTetrominos.Count)];
        nextTetrominos.Remove(Stored);
    }

}
