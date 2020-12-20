using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Camera))]
public class Camera : MonoBehaviour
{
    public Grid grid;
    private UnityEngine.Camera camera;

    private void Awake()
    {
        camera = GetComponent<UnityEngine.Camera>();

        float size = grid.height * grid.blockSize;
        camera.orthographicSize = size / 2 + size / 10;

        float xPos = grid.position.x + ((grid.length - 1) * grid.blockSize) / 2;
        float yPos = grid.position.y + grid.blockSize/2 - size/2;

        camera.transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
