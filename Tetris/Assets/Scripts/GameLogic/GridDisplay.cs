using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    private GameObject[,] gridDisplay;
    private Dictionary<GameObject, SpriteRenderer> blockDict = new Dictionary<GameObject, SpriteRenderer>();

    public void Create(int length, int height, Vector2 position, float blockSize, float outlinePercent, GameObject blockPrefab, Sprite noBlockSprite, Transform blocksParent)
    {
        gridDisplay = new GameObject[height, length];

        Transform blocks = (new GameObject("Blocks")).transform;
        blocks.parent = blocksParent;

        Vector2 currPos = position;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                GameObject gameBlock = Instantiate(blockPrefab, currPos, Quaternion.identity);
                gameBlock.transform.parent = blocks.transform;

                gameBlock.name = "Block(" + x + ", " + y + ")";
                SpriteRenderer spriteRenderer = gameBlock.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = noBlockSprite;
                blockDict.Add(gameBlock, spriteRenderer);

                float spriteSize = spriteRenderer.bounds.size.x;
                float size = (blockSize * outlinePercent) / spriteSize;
                gameBlock.transform.localScale = new Vector3(size, size, 1);
                gridDisplay[y, x] = gameBlock;

                GameObject backgroundBlock = Instantiate(blockPrefab, new Vector3(currPos.x, currPos.y, 1), Quaternion.identity);
                backgroundBlock.GetComponent<SpriteRenderer>().sprite = noBlockSprite;
                backgroundBlock.transform.localScale = new Vector3(size, size, 1);
                backgroundBlock.transform.parent = gameBlock.transform;

                currPos.x += blockSize;
            }
            currPos.y -= blockSize;
            currPos.x = position.x;
        }
    }

    public void SetSprite(int x, int y, Sprite sprite)
    {
        blockDict[gridDisplay[y, x]].sprite = sprite;
    }

    public void SetAlpha(int x, int y, float alpha)
    {
        Color color = blockDict[gridDisplay[y, x]].color;
        blockDict[gridDisplay[y, x]].color = new Color(color.r, color.g, color.b, alpha);
    }

    public void SetColor(int x, int y, Color color)
    {
        blockDict[gridDisplay[y, x]].color = color;
    }
}
