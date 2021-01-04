using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{

    [SerializeField]
    private int length;
    public int Length { get => length; }
    [SerializeField]
    private int height;
    public int Height { get => height; }
    [SerializeField]
    private int hiddenRows;
    public int HiddenRows { get => hiddenRows; }

    private Block[,] grid;

    public event Action<int, int, Block> GridChanged;

    public void Start()
    {
        grid = new Block[height + hiddenRows, length];
        Clear();
    }

    public List<int> FullRows()
    {
        List<int> fullRows = new List<int>();

        for (int y = 0; y < height + hiddenRows; y++)
        {
            bool fullRow = true;

            for (int x = 0; x < length; x++)
                if (!Get(x, y).IsSolid)
                    fullRow = false;

            if (fullRow) fullRows.Add(y);
        }

        return fullRows;
    }

    public bool InBounds(Vector2 pos) 
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        return InBounds(x, y);
    }


    public bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < length && y < height + hiddenRows;
    }

    public bool VacantPos(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        return InBounds(pos) && !Get(x, y).IsSolid;
    }

    public Block Get(int x, int y)
    {
        return InBounds(x, y) ? grid[y, x] : null;
    }

    public Block Get(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        return Get(x, y);
    }

    public void Set(int x, int y, Block block)
    {
        if (!InBounds(x, y)) return;

        grid[y, x] = block;

        GridChanged?.Invoke(x, y, block);
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
            for (int x = 0; x < length; x++)
                Set(x, y, Block.Empty());
    }
}
