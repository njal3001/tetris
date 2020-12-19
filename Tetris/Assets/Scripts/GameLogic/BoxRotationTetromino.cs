using UnityEngine;

public class BoxRotationTetromino : Tetromino
{
    private int boxSize;
    public BoxRotationTetromino(Vector2[] blocksPos, int boxSize, Block blockType) : base(blocksPos, blockType)
    {
        this.boxSize = boxSize;
    }

    protected override Vector2[] GetRotatedBlocksPos(int rotation)
    {
        if (rotation == 0)
        {
            return (Vector2[])blocksPos.Clone();
        }

        Vector2[] rotatedBlocksPos = new Vector2[4];
        for (int i = 0; i < blocksPos.Length; i++)
        {
            Vector2 pos = blocksPos[i];
            Vector2 rotatedPos = new Vector2();

            switch (rotation)
            {
                case 1:
                    rotatedPos.x = boxSize - 1 - pos.y;
                    rotatedPos.y = pos.x;
                    break;
                case 2:
                    rotatedPos.x = boxSize - 1 - pos.x;
                    rotatedPos.y = boxSize - 1 - pos.y;
                    break;
                case 3:
                    rotatedPos.x = pos.y;
                    rotatedPos.y = boxSize - 1 - pos.x;
                    break;
            }
            rotatedBlocksPos[i] = rotatedPos;
        }
        return rotatedBlocksPos;
    }

    protected override void InitializeWallKickData()
    {
        wallKickData[0, 1] = new Vector2[] { new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, 1), new Vector2(1, -2) };
        wallKickData[1, 0] = new Vector2[] { new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, -1), new Vector2(-1, 2) };
        wallKickData[1, 2] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, -2), new Vector2(2, 1) };
        wallKickData[2, 1] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, 2), new Vector2(-2, -1) };
        wallKickData[2, 3] = new Vector2[] { new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, -1), new Vector2(-1, 2) };
        wallKickData[3, 2] = new Vector2[] { new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, 1), new Vector2(1, -2) };
        wallKickData[3, 0] = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, 2), new Vector2(-2, -1) };
        wallKickData[0, 3] = new Vector2[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, -2), new Vector2(2, 1) };
    }
}
