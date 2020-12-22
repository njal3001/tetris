using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [Header("Grid Properties")]
    public int length;
    public int height;
    public int hiddenRows;
    private Block[,] grid;

    [Header("Display Properties")]
    public Vector2 position;
    public float blockSize;
    public float outlinePercent;
    public GameObject blockPrefab;
    public Sprite noBlockSprite;
    public float ghostBlockAlpha;
    public GridDisplay gridDisplay;

    [Header("Effects")]
    public AudioManager audioManager;
    public float clearRowEffectTime = 1;
    private bool clearRowsEffectPlaying;
    private List<int> clearedRowsY;
    private int clearBlockLeftXPos;
    private int clearBlockRightXPos;
    private float timeToNextClearBlock;
    private float currTimeToNextClearBlock;

    public void Start()
    {
        grid = new Block[height + hiddenRows, length];
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        gridDisplay.Create(length, height, position, blockSize, outlinePercent, blockPrefab, noBlockSprite, transform);
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

        audioManager.Play("rowClear");
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
            Block block = grid[y, x];
            Set(x, y, null);
            Set(x, y + 1, block);
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

    public Block Get(int x, int y)
    {
        return grid[y, x];
    }

    public Block Get(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        return Get(x, y);
    }

    public void Set(int x, int y, Block block)
    {
        grid[y, x] = block;

        if (y >= hiddenRows)
        {
            Sprite sprite= block == null ? noBlockSprite : block.Sprite;
            gridDisplay.SetSprite(x, y - hiddenRows, sprite);
            gridDisplay.SetAlpha(x, y - hiddenRows, 1f);
        }
    }

    public void Set(Vector2 pos, Block block)
    {
        int x = (int)pos.x; 
        int y = (int)pos.y;

        Set(x, y, block);
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
