using UnityEngine;

public class BoxRotationTetromino : Tetromino
{
    [SerializeField]
    private int boxSize;

    protected override Vector2 Rotate(Vector2 oldPos, bool toRight)
    {
        Vector2 rotatedPos = new Vector2();

        if (toRight)
        {
            rotatedPos.x = boxSize - 1 - oldPos.y;
            rotatedPos.y = oldPos.x;
        }
        else
        {
            rotatedPos.x = oldPos.y;
            rotatedPos.y = boxSize - 1 - oldPos.x;
        }

        return rotatedPos;
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
