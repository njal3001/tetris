using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [Header("Grid Properties")]
    public int length;
    public int height;
    public int hiddenRows;
    private Sprite[,] grid;

    [Header("Display Properties")]
    public float blockSize;
    public float outlinePercent;
    public GameObject blockPrefab;
    public Sprite noBlockSprite;
    public float ghostBlockAlpha;
    public GridDisplay gridDisplay;

    [Header("Effects")]
    public float clearRowEffectTime = 1;
    private bool clearRowsEffectPlaying;
    private List<int> clearedRowsY;
    private int clearBlockLeftXPos;
    private int clearBlockRightXPos;
    private float timeToNextClearBlock;
    private float currTimeToNextClearBlock;

    public void Start()
    {
        grid = new Sprite[height + hiddenRows, length];
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        gridDisplay.Create(length, height, blockSize, outlinePercent, blockPrefab, noBlockSprite);  
    }

    public void StartClearedRowsEffect()
    {
        clearRowsEffectPlaying = true;

        timeToNextClearBlock = clearRowEffectTime / Mathf.Floor((length / 2f));

        if (length % 2 == 0)
        {
            clearBlockLeftXPos = (length / 2) - 1;
            clearBlockRightXPos = clearBlockLeftXPos + 1;
        }
        else
        {
            clearBlockLeftXPos = clearBlockRightXPos = length / 2;
        }

    }

    public bool ClearRowsEffectPlaying()
    {
        return clearRowsEffectPlaying;
    }

    private void Update()
    {
        currTimeToNextClearBlock += Time.deltaTime;
        if (clearRowsEffectPlaying && currTimeToNextClearBlock >= timeToNextClearBlock)
        {
            currTimeToNextClearBlock = 0;
            foreach (int y in clearedRowsY)
            {
                Set(clearBlockLeftXPos, y, null);
                Set(clearBlockRightXPos, y, null);
            }

            clearBlockLeftXPos--;
            clearBlockRightXPos++;

            if (clearBlockLeftXPos < 0)
            {
                foreach (int y in clearedRowsY)
                {
                    for (int i = y - 1; i >= 0; i--)
                    {
                        MoveRowDown(i);
                    }
                }

                clearRowsEffectPlaying = false;
            }
        }
    }

    public int ClearFullRows()
    {
        clearedRowsY = new List<int>();

        for (int y = hiddenRows; y < height + hiddenRows; y++)
        {
            bool clearRow = true;

            for (int x = 0; x < length; x++)
            {
                if (grid[y, x] == null)
                {
                    clearRow = false;
                }
            }

            if (clearRow)
            {
                clearedRowsY.Add(y);
            }
        }

        if (clearedRowsY.Count > 0)
        {
            StartClearedRowsEffect();
        }
        return clearedRowsY.Count;
    }

    private void MoveRowDown(int y)
    {
        for (int x = 0; x < length; x++)
        {
            Sprite sprite = grid[y, x];
            Set(x, y, null);
            Set(x, y + 1, sprite);
        }
    }

    public bool InBounds(Vector2 pos) 
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        return x >= 0 && y >= 0 && x < length && y < height + hiddenRows;
    }

    public bool VacantPos(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        return InBounds(pos) && grid[y, x] == null;
    }

    public Sprite Get(int x, int y)
    {
        return grid[y, x];
    }

    public Sprite Get(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        return Get(x, y);
    }

    public void Set(int x, int y, Sprite sprite)
    {
        grid[y, x] = sprite;
        if (y >= hiddenRows)
        {
            Sprite displaySprite = sprite == null ? noBlockSprite : sprite;
            gridDisplay.SetSprite(x, y - hiddenRows, displaySprite);
            gridDisplay.SetAlpha(x, y - hiddenRows, 1f);
        }
    }

    public void Set(Vector2 pos, Sprite sprite)
    {
        int x = (int)pos.x; 
        int y = (int)pos.y;

        Set(x, y, sprite);
    }

    public void Clear()
    {
        for (int y = 0; y < height + hiddenRows; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Set(x, y, null);
            }
        }
    }
}
