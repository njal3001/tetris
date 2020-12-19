using UnityEngine;

public class BlockRotationTetromino : Tetromino
{

    private Vector2 pivot;

    public BlockRotationTetromino(Vector2[] blocksPos, Vector2 pivot, Block blockType) : base(blocksPos, blockType)
    {
        this.pivot = pivot;
    } 

    protected override Vector2[] GetRotatedBlocksPos(int rotation)
    {
        if (rotation == 0)
        {
            return (Vector2[])blocksPos.Clone();
        }

        float s = 0;
        float c = 0;
        switch(rotation)
        {
            case 1:
                s = -1;
                c = 0;
                break;
            case 2:
                s = 0;
                c = -1;
                break;
            case 3:
                s = 1;
                c = 0;
                break;
        }

        Vector2[] rotatedBlocksPos = new Vector2[blocksPos.Length];

        for (int i = 0; i < rotatedBlocksPos.Length; i++)
        {
            Vector2 pos = blocksPos[i];

            pos -= pivot;
            pos.y *= -1;
            Vector2 rotatedPos = new Vector2();
            rotatedPos.x = pos.x * c - pos.y * s;
            rotatedPos.y = pos.y * c + pos.x * s;

            rotatedPos.y *= -1;
            rotatedPos += pivot;

            rotatedBlocksPos[i] = rotatedPos;
        }
        return rotatedBlocksPos;
    }

    

    protected override void InitializeWallKickData()
    {
        wallKickData[0, 1] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2) };
        wallKickData[1, 0] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2) };
        wallKickData[1, 2] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2) };
        wallKickData[2, 1] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2) };
        wallKickData[2, 3] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2) };
        wallKickData[3, 2] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2) };
        wallKickData[3, 0] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2) };
        wallKickData[0, 3] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2) };
    }
}
