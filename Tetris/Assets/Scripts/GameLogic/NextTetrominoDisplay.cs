using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTetrominoDisplay : MonoBehaviour
{
    public Grid grid;
    public GridDisplay display;

    public UnityEngine.Camera cam;


    public void Start()
    {
        float posX = cam.transform.position.x + cam.orthographicSize;
        float posY = cam.transform.position.y;

        display.Create(4, 2, new Vector2(posX, posY), grid.blockSize, grid.outlinePercent, grid.blockPrefab, grid.noBlockSprite, transform);
    }

    public void Display(Tetromino tetromino)
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                display.SetSprite(x, y, grid.noBlockSprite);
            }
        }

        foreach (Vector2 pos in tetromino.BlocksPos)
        {
            display.SetSprite((int)pos.x, (int)pos.y, tetromino.BlockType.Sprite);
        }
    }

}
