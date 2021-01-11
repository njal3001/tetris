using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoGenerator : TetrominoStorer
{

    [SerializeField]
    private Tetromino[] tetrominos;

    private List<Tetromino> nextTetrominos = new List<Tetromino>();

    public event Action Initialized;

    protected override void OnEnable()
    {
        base.OnEnable();
        tetrisState.OnGameStarted += Initialize;
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
            nextTetrominos.AddRange(tetrominos);

        Stored = nextTetrominos[UnityEngine.Random.Range(0, nextTetrominos.Count)];
        nextTetrominos.Remove(Stored);
    }

    private void Initialize()
    {
        nextTetrominos.Clear();
        UpdateNextTetromino();
        Initialized?.Invoke();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        tetrisState.OnGameStarted -= Initialize;
    }

}
