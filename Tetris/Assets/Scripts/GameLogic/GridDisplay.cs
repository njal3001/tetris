using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    private GameObject[,] gridDisplay;
    private Dictionary<GameObject, SpriteRenderer> blockDict = new Dictionary<GameObject, SpriteRenderer>();

    public GameObject backgroundPrefab;

    public void Create(int length, int height, float blockSize, float outlinePercent, GameObject blockPrefab, Sprite noBlockSprite)
    {
        gridDisplay = new GameObject[height, length];

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Transform blocks = (new GameObject("Blocks")).transform;
        blocks.parent = transform;

        Vector2 currPos = transform.position;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                GameObject gameBlock = Instantiate(blockPrefab, currPos, Quaternion.identity, blocks.transform);

                gameBlock.name = "Block(" + x + ", " + y + ")";
                SpriteRenderer spriteRenderer = gameBlock.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = noBlockSprite;
                blockDict.Add(gameBlock, spriteRenderer);

                float spriteSize = spriteRenderer.bounds.size.x;
                float size = (blockSize * outlinePercent) / spriteSize;
                gameBlock.transform.localScale = new Vector3(size, size, 1);
                gridDisplay[y, x] = gameBlock;

                GameObject backgroundBlock = Instantiate(blockPrefab, new Vector3(currPos.x, currPos.y, 1), Quaternion.identity, gameBlock.transform);
                backgroundBlock.GetComponent<SpriteRenderer>().sprite = noBlockSprite;
                backgroundBlock.transform.localScale = new Vector3(1, 1, 1);

                currPos.x += blockSize;
            }
            currPos.y -= blockSize;
            currPos.x = transform.position.x;
        }

        if (backgroundPrefab == null) return;

        float backgroundXPos = transform.position.x + (length / 2f) * blockSize - (blockSize / 2f);
        float backgroundYPos = transform.position.y - (height / 2f) * blockSize + (blockSize / 2f);
        Vector3 backgroundPos = new Vector3(backgroundXPos, backgroundYPos, 5);

        GameObject background = Instantiate(backgroundPrefab, backgroundPos, Quaternion.identity, transform);
        background.name = "Background";

        background.transform.localScale = new Vector3(length * blockSize, height * blockSize, 1);

    }

    public void SetSprite(int x, int y, Sprite sprite)
    {
        if (OutOfBounds(x, y)) return;

        blockDict[gridDisplay[y, x]].sprite = sprite;
    }

    public void SetAlpha(int x, int y, float alpha)
    {
        if (OutOfBounds(x, y)) return;

        Color color = blockDict[gridDisplay[y, x]].color;
        blockDict[gridDisplay[y, x]].color = new Color(color.r, color.g, color.b, alpha);
    }

    private bool OutOfBounds(int x, int y)
    {
        return x < 0 || y < 0 || x >= gridDisplay.GetLength(1) || y >= gridDisplay.GetLength(0);
    }

}
