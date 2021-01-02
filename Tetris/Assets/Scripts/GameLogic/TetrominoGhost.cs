using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoGhost : MonoBehaviour
{
    [SerializeField] private Tetromino tetromino;
    private GridDisplay gridDisplay;

    private List<Vector2> oldGhostPos = new List<Vector2>();

    private void OnEnable()
    {
        tetromino.OnPosChanged += UpdateGhost;
    }

    private void Start()
    {
        gridDisplay = FindObjectOfType<GridDisplay>();
    }

    public TetrominoGhost(Tetromino tetromino)
    {
        tetromino.OnPosChanged += UpdateGhost;
    }

    public void UpdateGhost(Vector2[] newPos)
    {
        
    }

    public void OnDisable()
    {
        tetromino.OnPosChanged -= UpdateGhost;
    }

}
