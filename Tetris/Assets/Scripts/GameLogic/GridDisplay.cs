using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    private GameObject[,] gridDisplay;
    private Dictionary<GameObject, Material> blockDict = new Dictionary<GameObject, Material>();

    public GridDisplay(int length, int height, Vector2 position, float blockSize, float blockSpace, GameObject blockPrefab, Color noBlockColor)
    {
        gridDisplay = new GameObject[height, length];

        Vector2 currPos = position;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                GameObject gameBlock = Instantiate(blockPrefab, currPos, Quaternion.identity);
                gameBlock.name = "Block(" + x + ", " + y + ")";
                gameBlock.transform.localScale = new Vector3(blockSize, blockSize, 1);
                Material material = gameBlock.GetComponent<MeshRenderer>().material;
                material.color = noBlockColor;
                blockDict.Add(gameBlock, material);
                gridDisplay[y, x] = gameBlock;

                currPos.x += blockSize + blockSpace;
            }
            currPos.y -= blockSize + blockSpace;
            currPos.x = position.x;
        }
    }

    public void Set(int x, int y, Color color)
    {
        blockDict[gridDisplay[y, x]].color = color;
    }
}
