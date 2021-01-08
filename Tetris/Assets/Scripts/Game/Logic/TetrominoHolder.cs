
using System;

public class TetrominoHolder : TetrominoStorer
{

    public event Action<Tetromino> TetrominoHeld;

    public void Hold(Tetromino tetromino)
    {
        Tetromino prevHolding = base.Stored;
        tetromino?.Clear();
        base.Stored = tetromino;
        TetrominoHeld?.Invoke(prevHolding);
    }
}
