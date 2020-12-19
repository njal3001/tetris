using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [Header("Grid Properties")]
    public int length;
    public int height;
    public int hiddenRows;
    public Vector2 position;

    private Block[,] grid;
    private GameObject[,] gameGrid;
    private Dictionary<GameObject, Material> blockDict = new Dictionary<GameObject, Material>();

    [Header("Block Properties")]
    public float blockSize;
    public float blockSpace;
    public GameObject blockPrefab;
    public Color noBlockColor;

    [Header("Effects")]
    public float clearFullRowFadeTime = 1;
    private bool clearedRowsFading;
    private List<int> clearedRowsY = new List<int>();
    private float fadeProgress;

    private void Start()
    {
        grid = new Block[height + hiddenRows, length];
        gameGrid = new GameObject[height, length];
        CreateGameGrid();
    }

    private void Update()
    {
        if (clearedRowsFading)
        {
            fadeProgress += Time.deltaTime / clearFullRowFadeTime;
            foreach (int y in clearedRowsY)
            {
                for (int x = 0; x < length; x++)
                {
                    Color color = Color.Lerp(Get(x, y).Color, noBlockColor, fadeProgress);
                    UpdateGameGrid(x, y, color);
                }
            }

            if (fadeProgress >= 1)
            {
                foreach (int y in clearedRowsY)
                {
                    for (int i = y - 1; i >= 0; i--)
                    {
                        MoveRowDown(i);
                    }
                }

                fadeProgress = 0;
                clearedRowsFading = false;
                clearedRowsY.Clear();
            }
        }
    }

    private void CreateGameGrid()
    {
        Vector2 pos = position;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                GameObject gameBlock = Instantiate(blockPrefab, pos, Quaternion.identity);
                gameBlock.name = "Block(" + x + ", " + y + ")";
                gameBlock.transform.localScale = new Vector3(blockSize, blockSize, 1);
                Material material = gameBlock.GetComponent<MeshRenderer>().material;
                material.color = noBlockColor;
                blockDict.Add(gameBlock, material);
                gameGrid[y, x] = gameBlock;

                pos.x += blockSize + blockSpace;
            }
            pos.y -= blockSize + blockSpace;
            pos.x = position.x;
        }
    }

    public int ClearFullRows()
    {
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
            clearedRowsFading = true;
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

        Color color = block == null ? noBlockColor : block.Color;
        UpdateGameGrid(x, y, color);
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

    //Accounts for the hidden rows offset
    private void UpdateGameGrid(int x, int y, Color color)
    {
        if (y >= hiddenRows)
        {
            blockDict[gameGrid[y - hiddenRows, x]].color = color;
        }
    }

    private void FullRowFade(int y)
    {

    }
}
