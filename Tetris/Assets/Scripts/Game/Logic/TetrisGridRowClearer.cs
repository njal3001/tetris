﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGridRowClearer : MonoBehaviour
{
    [SerializeField]
    private TetrisGrid grid;

    [SerializeField]
    private float clearingTime;

    public event Action <int>RowsCleared;
    public event Action TetrominoCanSpawn;

    public int ClearFullRows()
    {
        List<int> fullRows = grid.FullRows();
 
        if (fullRows.Count == 0)
        {
            TetrominoCanSpawn?.Invoke();
            return 0;
        }

        RowsCleared?.Invoke(fullRows.Count);
        StartCoroutine(ClearFullRows(grid.FullRows()));
        return fullRows.Count;
    }

    private IEnumerator ClearFullRows(List<int> fullRows)
    {
        int iterations = (grid.Length + grid.Length % 2) / 2;
        float clearIntervalTime = clearingTime / iterations;
        float startingPoint = (grid.Length - 1) / 2f;

        for (int i = 0; i < iterations; i++)
        {
            foreach (int y in fullRows)
            {
                int left = Mathf.FloorToInt(startingPoint - i);
                int right = Mathf.CeilToInt(startingPoint + i);

                grid.Set(left, y, Block.Empty());
                grid.Set(right, y, Block.Empty());
            }
            yield return new WaitForSeconds(clearIntervalTime);
        }

        foreach (int y in fullRows)
            for (int j = y - 1; j >= 0; j--)
                MoveRowDown(j);

        TetrominoCanSpawn?.Invoke();
    }
    private void MoveRowDown(int y)
    {
        for (int x = 0; x < grid.Length; x++)
        {
            Block block = grid.Get(x, y);
            grid.Set(x, y, Block.Empty());
            grid.Set(x, y + 1, block);
        }
    }


}
