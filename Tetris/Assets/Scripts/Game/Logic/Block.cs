using UnityEngine;

public class Block
{
    private Block(Sprite sprite, bool isGhost)
    {
        Sprite = sprite;
        IsGhost = isGhost;
    }

    public Sprite Sprite
    {
        get;
    }

    public bool IsGhost
    {
        get;
    }

    public bool IsSolid
    {
        get
        {
            return !IsEmpty && !IsGhost;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Sprite == null;
        }
    }

    public static Block Empty()
    {
        return new Block(null, false);
    }

    public static Block Ghost(Sprite sprite)
    {
        return new Block(sprite, true);
    }

    public static Block Solid(Sprite sprite)
    {
        return new Block(sprite, false);
    }
}
