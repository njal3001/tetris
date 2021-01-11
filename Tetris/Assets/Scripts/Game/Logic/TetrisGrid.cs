using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    [SerializeField]
    private TetrisState tetrisState;

    [SerializeField]
    private int length;
    public int Length => length;
    [SerializeField]
    private int height;
    public int Height => height;
    [SerializeField]
    private int hiddenRows;
    public int HiddenRows => hiddenRows;

    private Block[,] grid;

    public event Action<int, int, Block> GridChanged;

    private void OnEnable()
    {
        tetrisState.OnClear += Clear;
    }

    public void Awake() => grid = new Block[height + hiddenRows, length];

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

    public bool InBounds(Vector2 point) 
    {
        int x = (int)point.x;
        int y = (int)point.y;
        return InBounds(x, y);
    }


    public bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < length && y < height + hiddenRows;
    }

    public bool Empty(Vector2 point)
    {
        int x = (int)point.x;
        int y = (int)point.y;
        return InBounds(point) && !Get(x, y).IsSolid;
    }

    public Block Get(int x, int y)
    {
        return InBounds(x, y) ? grid[y, x] : null;
    }

    public Block Get(Vector2 point)
    {
        int x = (int)point.x;
        int y = (int)point.y;

        return Get(x, y);
    }

    public void Set(int x, int y, Block block)
    {
        if (!InBounds(x, y)) return;

        grid[y, x] = block;

        GridChanged?.Invoke(x, y, block);
    }

    public void Set(Vector2 point, Block block)
    {
        int x = (int)point.x; 
        int y = (int)point.y;

        Set(x, y, block);
    }

    private void Clear()
    {
        for (int y = 0; y < height + hiddenRows; y++)
            for (int x = 0; x < length; x++)
                Set(x, y, Block.Empty());
    }

    private void OnDisable() => tetrisState.OnClear -= Clear;
}
