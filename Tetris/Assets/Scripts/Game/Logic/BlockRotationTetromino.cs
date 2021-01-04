using UnityEngine;

public class BlockRotationTetromino : Tetromino
{
    [SerializeField]
    private Vector2 pivot;

    protected override Vector2 Rotate(Vector2 oldPos, bool toRight)
    {
        float s = toRight ? -1 : 1;
        float c = 0;

        Vector2 pos = oldPos;

        pos -= pivot;
        pos.y *= -1;
        Vector2 rotatedPos = new Vector2();
        rotatedPos.x = pos.x * c - pos.y * s;
        rotatedPos.y = pos.y * c + pos.x * s;

        rotatedPos.y *= -1;
        rotatedPos += pivot;

        return rotatedPos;
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
