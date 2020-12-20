using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    private GameObject[,] gridDisplay;
    private Dictionary<GameObject, Material> blockDict = new Dictionary<GameObject, Material>();

    public GridDisplay(int length, int height, Vector2 position, float blockSize, float outlinePercent, GameObject blockPrefab, Color noBlockColor, Transform blocksParent)
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
                gameBlock.transform.localScale = new Vector3(blockSize * outlinePercent, blockSize * outlinePercent, 1);
                Material material = gameBlock.GetComponent<Renderer>().material;
                material.color = noBlockColor;
                blockDict.Add(gameBlock, material);
                gridDisplay[y, x] = gameBlock;

                currPos.x += blockSize;
            }
            currPos.y -= blockSize;
            currPos.x = position.x;
        }
    }

    public void Set(int x, int y, Color color)
    {
        blockDict[gridDisplay[y, x]].color = color;
    }

}
