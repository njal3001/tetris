using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoHolder : TetrominoStorer
{

    public Tetromino Swap(Tetromino tetromino)
    {
        Tetromino prevHolding = Stored;
        if (tetromino != null) tetromino.Clear();
        Stored = tetromino;
        return prevHolding;
    }
}
