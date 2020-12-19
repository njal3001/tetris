using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTetrominoDisplay : MonoBehaviour
{
    public Grid grid;
    public UnityEngine.Camera camera;

    private GridDisplay display;

    public void Start()
    {
        float posX = camera.transform.position.x + camera.orthographicSize;
        float posY = camera.transform.position.y;

        display = new GridDisplay(4, 2, new Vector2(posX, posY), grid.blockSize, grid.blockSpace, grid.blockPrefab, Color.white);
    }

    public void Display(Tetromino tetromino)
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                display.Set(x, y, camera.backgroundColor);
            }
        }

        foreach (Vector2 pos in tetromino.BlocksPos)
        {
            display.Set((int)pos.x, (int)pos.y, tetromino.BlockType.Color);
        }
    }

}
