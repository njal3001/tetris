using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoDisplay : MonoBehaviour
{
    public Grid grid;
    public GridDisplay display;

    public void Start()
    {
        CreateDisplay();
        Clear();
    }

    public void CreateDisplay()
    {
        display.Create(4, 2, grid.blockSize, grid.outlinePercent, grid.blockPrefab, grid.noBlockSprite);
    }

    public void Clear()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                display.SetAlpha(x, y, 0);
            }
        }
    }

    public void Display(Tetromino tetromino)
    {
        Clear();

        foreach (Vector2 pos in tetromino.BlocksPos)
        {
            int x = (int)pos.x;
            int y = (int)pos.y;
            display.SetSprite(x, y, tetromino.BlockType.Sprite);
            display.SetAlpha(x, y, 1);
        }
    }

}
