using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTetrominoDisplay : MonoBehaviour
{
    public Grid grid;
    public GridDisplay display;
    public Sprite whiteSprite;  

    public UnityEngine.Camera cam;


    public void Start()
    {
        float posX = cam.transform.position.x + cam.orthographicSize;
        float posY = cam.transform.position.y;

        display.Create(4, 2, new Vector2(posX, posY), grid.blockSize, grid.outlinePercent, grid.blockPrefab, whiteSprite, transform);
        Clear();
    }

    private void Clear()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                display.SetSprite(x, y, whiteSprite);
                display.SetColor(x, y, cam.backgroundColor);
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
