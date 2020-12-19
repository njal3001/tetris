using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera : MonoBehaviour
{
    public Grid grid;
    private UnityEngine.Camera camera;

    void Start()
    {
        camera = GetComponent<UnityEngine.Camera>();

        float size = grid.height * grid.blockSize + (grid.height - 1) * grid.blockSpace;
        camera.orthographicSize = size / 2 + 1;

        float xPos = grid.position.x + ((grid.length - 1) * grid.blockSize + (grid.length - 1) * grid.blockSpace) / 2;
        float yPos = grid.position.y + grid.blockSize/2 - size/2;

        camera.transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
