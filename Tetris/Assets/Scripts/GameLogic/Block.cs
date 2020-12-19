using UnityEngine;

public class Block
{

    private Color color;

    public Block(Color color)
    {
        this.color = color;
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }
}
