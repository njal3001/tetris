using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoDisplay : MonoBehaviour
{
    public Grid grid;
    public GridDisplay display;
    public Sprite whiteSprite;

    public void Start()
    {
        CreateDisplay();
        Clear();
    }

    public void CreateDisplay()
    {
        display.Create(4, 2, grid.blockSize, grid.outlinePercent, grid.blockPrefab, whiteSprite);
    }

    public void Clear()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                display.SetSprite(x, y, whiteSprite);
                display.SetColor(x, y, Color.black);
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
            display.SetColor(x, y, Color.white);
        }
    }

}
