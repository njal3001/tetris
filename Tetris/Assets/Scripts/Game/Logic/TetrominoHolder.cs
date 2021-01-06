using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoHolder : TetrominoStorer
{

    public Tetromino Hold(Tetromino tetromino)
    {
        Tetromino prevHolding = Stored;
        if (tetromino != null) tetromino.Clear();
        Stored = tetromino;
        return prevHolding;
    }
}
