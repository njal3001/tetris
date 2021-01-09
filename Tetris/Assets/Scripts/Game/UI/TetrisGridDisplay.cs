using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGridDisplay : MonoBehaviour
{

    [SerializeField]
    private TetrisGrid tetrisGrid;
    
    private GameObject[,] gridDisplay;
    private Dictionary<GameObject, SpriteRenderer> blockDict = new Dictionary<GameObject, SpriteRenderer>();

    [SerializeField]
    private float blockSize;
    [SerializeField]
    private float outlinePercent;
    [SerializeField]
    private float ghostBlockAlpha;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject backgroundPrefab;
    [SerializeField]
    private Sprite noBlockSprite;

    private void OnEnable() => tetrisGrid.GridChanged += OnGridChanged;

    private void Awake() => CreateDisplay();

    public void CreateDisplay()
    {
        gridDisplay = new GameObject[tetrisGrid.Height, tetrisGrid.Length];

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Transform blocks = (new GameObject("Blocks")).transform;
        blocks.parent = transform;

        Vector2 currPos = transform.position;
        for (int y = 0; y < tetrisGrid.Height; y++)
        {
            for (int x = 0; x < tetrisGrid.Length; x++)
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

        float backgroundXPos = transform.position.x + (tetrisGrid.Length / 2f) * blockSize - (blockSize / 2f);
        float backgroundYPos = transform.position.y - (tetrisGrid.Height / 2f) * blockSize + (blockSize / 2f);
        Vector3 backgroundPos = new Vector3(backgroundXPos, backgroundYPos, 5);

        GameObject background = Instantiate(backgroundPrefab, backgroundPos, Quaternion.identity, transform);
        background.name = "Background";

        background.transform.localScale = new Vector3(tetrisGrid.Length * blockSize, tetrisGrid.Height * blockSize, 1);
    }

    private void SetSprite(int x, int y, Sprite sprite)
    {
        if (OutOfBounds(x, y)) return;

        blockDict[gridDisplay[y, x]].sprite = sprite;

    }

    private void SetAlpha(int x, int y, float alpha)
    {
        if (OutOfBounds(x, y)) return;

        Color color = blockDict[gridDisplay[y, x]].color;
        blockDict[gridDisplay[y, x]].color = new Color(color.r, color.g, color.b, alpha);
    }

    private bool OutOfBounds(int x, int y)
    {
        return x < 0 || y < 0 || x >= gridDisplay.GetLength(1) || y >= gridDisplay.GetLength(0);
    }

    private void OnGridChanged(int x, int y, Block block)
    {
        if (y >= tetrisGrid.HiddenRows)
        {
            SetSprite(x, y - tetrisGrid.HiddenRows, block.IsEmpty ? noBlockSprite : block.Sprite);
            SetAlpha(x, y - tetrisGrid.HiddenRows, block.IsGhost ? ghostBlockAlpha : 1.0f);
        }
    }

    private void OnDisable() => tetrisGrid.GridChanged -= OnGridChanged;

}
