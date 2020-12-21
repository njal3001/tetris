using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tetromino
{
    private Block blockType;
    protected Vector2[] blocksPos = new Vector2[4];
    private Grid grid;

    private Vector2[] currBlocksPos;
    private Vector2 relativeOrigin;
    private int rotation;   
    protected Vector2[,][] wallKickData = new Vector2[4,4][];

    private List<Vector2> oldGhostBlocksPos;

    public Tetromino(Vector2[] blocksPos, Block blockType)
    {
        Array.Copy(blocksPos, this.blocksPos, 4);
        this.blockType = blockType;
        InitializeWallKickData();
    }

    public bool Spawn(Vector2 relativeOrigin, Grid grid)
    {
        this.relativeOrigin = relativeOrigin;
        this.grid = grid;
        currBlocksPos = blocksPos;

        //Checks if tetromino can be spawned
        Vector2[] gridBlocksPos = ToGridPos(currBlocksPos);

        foreach (Vector2 pos in gridBlocksPos)
        {
            if (!grid.VacantPos(pos))
            {
                return false;
            }
        }

        //Spawns tetromino
        foreach (Vector2 pos in gridBlocksPos)
        {
            grid.Set(pos, blockType);
        }

        this.relativeOrigin = relativeOrigin;
        currBlocksPos = blocksPos;
        rotation = 0;

        oldGhostBlocksPos = new List<Vector2>();
        DrawGhost();

        return true;
    }

    public bool Move(Vector2 moveAmount)
    {
        Vector2[] gridBlocksPos = ToGridPos(currBlocksPos);

        Vector2[] newGridBlocksPos = new Vector2[gridBlocksPos.Length];

        //Checks if tetromino can be moved
        for (int i = 0; i < gridBlocksPos.Length; i++)
        {
            newGridBlocksPos[i] = gridBlocksPos[i] + moveAmount;

            if (!grid.InBounds(newGridBlocksPos[i]) || (grid.Get(newGridBlocksPos[i]) != null && !gridBlocksPos.Contains(newGridBlocksPos[i])))
            {
                return false;   
            }
        }

        //Moves the tetromino
        foreach (Vector2 pos in gridBlocksPos)
        {
            grid.Set(pos, null);
        }

        foreach (Vector2 newPos in newGridBlocksPos)
        {
            grid.Set(newPos, blockType);
        }

        relativeOrigin += moveAmount;
        DrawGhost();

        return true;
    }



    public bool CanMove(Vector2 moveAmount)
    {
        Vector2[] gridBlocksPos = ToGridPos(currBlocksPos);

        Vector2[] newGridBlocksPos = new Vector2[gridBlocksPos.Length];

        //Checks if tetromino can be moved
        for (int i = 0; i < gridBlocksPos.Length; i++)
        {
            newGridBlocksPos[i] = gridBlocksPos[i] + moveAmount;
            if (!grid.InBounds(newGridBlocksPos[i]) || (grid.Get(newGridBlocksPos[i]) != null && !gridBlocksPos.Contains(newGridBlocksPos[i])))
            {
                return false;
            }
        }

        return true;
    }

    public bool Rotate(bool toRight)
    {
        int newRotation = (rotation + (toRight ? 1 : -1)) % 4;

        if (newRotation < 0)
        {
            newRotation += 4;
        }

        foreach (Vector2 kick in wallKickData[rotation, newRotation])
        {
            Vector2[] gridBlocksPos = ToGridPos(currBlocksPos);

            Vector2[] rotatedBlocksPos = GetRotatedBlocksPos(newRotation);
            Vector2[] rotatedGridBlocksPos = ToGridPos(rotatedBlocksPos, relativeOrigin + kick);

            bool skip = false;
            //Checks if tetromino can be rotated
            foreach (Vector2 rotatedPos in rotatedGridBlocksPos)
            {
                if (!grid.InBounds(rotatedPos) || (grid.Get(rotatedPos) != null && !gridBlocksPos.Contains(rotatedPos)))
                {
                    skip = true;
                }
            }

            if (skip) continue;

            //Rotates the tetromino
            foreach (Vector2 pos in gridBlocksPos)
            {
                grid.Set(pos, null);
            }
            foreach (Vector2 newPos in rotatedGridBlocksPos)
            {
                grid.Set(newPos, blockType);
            }

            currBlocksPos = rotatedBlocksPos;
            rotation = newRotation;
            relativeOrigin += kick;
            DrawGhost();

            return true;
        }

        return false;
    }

    public void HardDrop()
    {
        int dist = HardDropDistance();

        if (dist > 0)
        {
            Move(new Vector2(0, dist));
        }
    }

    public int HardDropDistance()
    {
        int dist = 1;
        while (CanMove(new Vector2(0, dist)))
        {
            dist++;
        }

        return dist - 1;
    }

    private void DrawGhost()
    {
        //Clearing old ghost
        foreach (Vector2 oldPos in oldGhostBlocksPos)
        {
            if (!ToGridPos(currBlocksPos).Contains(oldPos))
            {
                int x = (int)oldPos.x;
                int y = (int)oldPos.y - grid.hiddenRows;
                grid.gridDisplay.SetSprite(x, y, grid.noBlockSprite);
                grid.gridDisplay.SetAlpha(x, y, 1);
            }
        }

        //Drawing new ghost
        int dist = HardDropDistance();
        if (dist == 0) return;

        Vector2[] currGridBlocksPos = ToGridPos(currBlocksPos);
        foreach (Vector2 currPos in currGridBlocksPos)
        {
            Vector2 ghostPos = currPos + new Vector2(0, dist);

            if (!currGridBlocksPos.Contains(ghostPos))
            {
                int x = (int)ghostPos.x;
                int y = (int)ghostPos.y - grid.hiddenRows;
                grid.gridDisplay.SetSprite(x, y, blockType.Sprite);
                grid.gridDisplay.SetAlpha(x, y, grid.ghostBlockAlpha);

                oldGhostBlocksPos.Add(ghostPos);
            }
        }
    }

    //Converts the local block positions to their position in the grid
    private Vector2[] ToGridPos(Vector2[] blocksPos, Vector2 relativeOrigin)
    {
        Vector2[] gridBlocksPos = new Vector2[blocksPos.Length];

        for (int i = 0; i < blocksPos.Length; i++)
        {
            gridBlocksPos[i] = blocksPos[i] + relativeOrigin;
        }
        return gridBlocksPos;
    }

    private Vector2[] ToGridPos(Vector2[] blocksPos)
    {
        return ToGridPos(blocksPos, relativeOrigin);
    }

    public bool OutOfSight()
    {
        foreach (Vector2 pos in ToGridPos(currBlocksPos))
        {
            if (pos.y < grid.hiddenRows)
            {
                return true;
            }
        }
        return false;
    }

    public Block BlockType
    {
        get { return blockType; }
    }

    public Vector2[] BlocksPos
    {
        get { return (Vector2[])blocksPos.Clone(); } 
    }

    protected abstract Vector2[] GetRotatedBlocksPos(int rotation);

    protected abstract void InitializeWallKickData();

}
