using System;
using System.Collections;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{

    [SerializeField]
    private Vector2 spawnPoint;

    [SerializeField]
    private TetrominoGenerator tetrominoGenerator;

    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;

    public event Action<Tetromino> TetrominoSpawned;

    private void OnEnable()
    {
        tetrominoGenerator.Initialized += SpawnNextTetromino;
        gridRowClearer.RowClearingFinished += SpawnNextTetromino;
        tetrominoHolder.TetrominoHeld += OnTetrominoHeld;
    }

    public void SpawnNextTetromino() => StartCoroutine(SpawnTetromino(tetrominoGenerator.NextTetromino));

    private void OnTetrominoHeld(Tetromino prevHolding)
    {
        if (prevHolding == null) SpawnNextTetromino();
        else StartCoroutine(SpawnTetromino(prevHolding));
    }

    private IEnumerator SpawnTetromino(Tetromino tetromino)
    {
        //Quick fix... Needs to wait an extra frame to ensure that TetrominoFinish event methods are completed
        yield return null;

        tetromino.Spawn(spawnPoint);
        TetrominoSpawned?.Invoke(tetromino);
    } 

    private void OnDisable()
    {
        tetrominoGenerator.Initialized -= SpawnNextTetromino;
        gridRowClearer.RowClearingFinished -= SpawnNextTetromino;
        tetrominoHolder.TetrominoHeld -= OnTetrominoHeld;
    }

}
