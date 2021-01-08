using System;
using System.Linq;
using UnityEngine;

public abstract class Tetromino : MonoBehaviour
{
    [SerializeField]
    protected Vector2[] relativeSpawnPos = new Vector2[4];
    [SerializeField]
    private Sprite blockSprite;
    private Block block;
    public Block Block { get => block; }
    private TetrisGrid grid;

    [SerializeField]
    private Vector2[] relativePos;
    private Vector2 relativeOrigin;
    private int rotation;   
    protected Vector2[,][] wallKickData = new Vector2[4,4][];

    public event Action<Vector2[]> PosChanged;


    //Using a delegate for moving or rotating the tetromino
    private delegate Vector2 CalculateDelegate(Vector2 oldPos);

    //Applies the calculation to all points in the position
    private Vector2[] Calculate(Vector2[] pos, CalculateDelegate calculation)
    {
        int length = pos == null ? 0 : pos.Length;
        Vector2[] calculatedPos = new Vector2[length];

        for (int i = 0; i < length; i++)
            calculatedPos[i] = calculation(pos[i]);

        return calculatedPos;
    }

    private Vector2 Move(Vector2 point, Vector2 moveAmount)
    {
        return point + moveAmount;
    }

    private Vector2[] Move(Vector2[] pos, Vector2 moveAmout)
    {
        return Calculate(pos, (Vector2 point) => Move(point, moveAmout));
    }

    protected abstract Vector2 Rotate(Vector2 point, bool toRight);

    private Vector2[] Rotate(Vector2[] pos, bool toRight)
    {
        return Calculate(pos, (Vector2 point) => Rotate(point, toRight));
    }


    private void Awake()
    {
        grid = FindObjectOfType<TetrisGrid>();
        block = Block.Solid(blockSprite);
        InitializeWallKickData();
    }

    private bool ValidNewPos(Vector2[] oldPos, Vector2[] newPos)
    {
        foreach (Vector2 point in newPos)
            if (!grid.InBounds(point) || (grid.Get(point).IsSolid && !oldPos.Contains(point)))
                return false;

        return true;
    }

    public bool Spawn(Vector2 spawnPoint)
    {
        Vector2[] newPos = Move(relativeSpawnPos, spawnPoint);

        foreach (Vector2 point in newPos)
            if (!grid.Empty(point)) return false;

        relativeOrigin = spawnPoint;
        relativePos = relativeSpawnPos;
        rotation = 0;

        UpdatePos(Array.Empty<Vector2>(), newPos);

        return true;
    }

    public bool Move(Vector2 moveAmount)
    {
        Vector2[] oldPos = Move(relativePos, relativeOrigin);
        Vector2[] newPos = Move(oldPos, moveAmount);

        if (!ValidNewPos(oldPos, newPos)) return false;

        relativeOrigin += moveAmount;

        UpdatePos(oldPos, newPos);

        return true;
    }

    private void UpdatePos(Vector2[] oldPos, Vector2[] newPos)
    {
        foreach (Vector2 point in oldPos)
            grid.Set(point, Block.Empty());

        foreach (Vector2 point in newPos)
            grid.Set(point, block);

        PosChanged?.Invoke(newPos);
    }


    public bool CanMove(Vector2 moveAmount)
    {
        Vector2[] oldPos = Move(relativePos, relativeOrigin);
        Vector2[] newPos = Move(oldPos, moveAmount);

        return ValidNewPos(oldPos, newPos);
    }

    public bool Rotate(bool toRight)
    {
        int newRotation = (rotation + (toRight ? 1 : 3)) % 4;

        Vector2[] oldPos = Move(relativePos, relativeOrigin);
        Vector2[] rotatedPos = Rotate(relativePos, toRight);

        foreach (Vector2 kickAmount in wallKickData[rotation, newRotation])
        {
            Vector2[] newPos = Move(rotatedPos, relativeOrigin + kickAmount);

            if (!ValidNewPos(oldPos, newPos)) continue;

            relativePos = rotatedPos;
            rotation = newRotation;
            relativeOrigin += kickAmount;

            UpdatePos(oldPos, newPos);

            return true;
        }
        return false;
    }

    public void HardDrop()
    {
        int dist = FallDistance();

        if (dist > 0)
        {
            Move(new Vector2(0, dist));
        }
    }

    public int FallDistance()
    {
        int dist = 1;
        while (CanMove(new Vector2(0, dist))) dist++;

        return dist - 1;
    }

    public void Clear()
    {
        UpdatePos(Move(relativePos, relativeOrigin), Array.Empty<Vector2>());
    }

    public bool OutOfSight()
    {
        foreach (Vector2 point in Move(relativePos, relativeOrigin))
            if (point.y < grid.HiddenRows)
                return true;

        return false;
    }

    protected abstract void InitializeWallKickData();

}
