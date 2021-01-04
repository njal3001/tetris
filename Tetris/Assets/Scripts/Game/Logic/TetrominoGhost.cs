using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrominoGhost : MonoBehaviour
{
    [SerializeField] 
    private Tetromino tetromino;

    private TetrisGrid grid;

    private List<Vector2> oldGhostPos = new List<Vector2>();

    private void OnEnable()
    {
        tetromino.PosChanged += UpdateGhost;
    }

    private void Awake()
    {
        grid = FindObjectOfType<TetrisGrid>();
    }

    public void UpdateGhost(Vector2[] newPos)
    {
        ClearGhost();

        int dist = tetromino.FallDistance();
        if (dist == 0) return;

        foreach (Vector2 pos in newPos)
        {
            Vector2 ghostPos = pos + new Vector2(0, dist);

            if (!newPos.Contains(ghostPos))
            {
                grid.Set(ghostPos, Block.Ghost(tetromino.Block.Sprite));
                oldGhostPos.Add(ghostPos);
            }
        }
    }

    private void ClearGhost()
    {
        foreach (Vector2 oldPos in oldGhostPos)
            if (grid.Get(oldPos).IsGhost)
                grid.Set(oldPos, Block.Empty());

        oldGhostPos.Clear();
    }

    public void OnDisable()
    {
        tetromino.PosChanged -= UpdateGhost;
    }

}
