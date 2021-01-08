using System;
using System.Collections;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{

    [SerializeField]
    private Vector2 spawnPoint;

    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    private Tetris tetris;

    [SerializeField]
    private TetrominoGenerator tetrominoGenerator;

    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;

    public event Action<Tetromino> TetrominoSpawned;

    private void OnEnable()
    {
        tetris.GameStarted += SpawnNextTetromino;
        gridRowClearer.TetrominoCanSpawn += SpawnNextTetromino;
        tetrominoHolder.TetrominoHeld += OnTetrominoHeld;
    }

    public void SpawnNextTetromino()
    {
        StartCoroutine(SpawnTetromino(tetrominoGenerator.NextTetromino));
    }

 
    private void OnTetrominoHeld(Tetromino prevHolding)
    {
        if (prevHolding == null) SpawnNextTetromino();
        else StartCoroutine(SpawnTetromino(prevHolding));
    }

    private IEnumerator SpawnTetromino(Tetromino tetromino)
    {
        yield return new WaitForSeconds(spawnDelay);

        tetromino.Spawn(spawnPoint);
        TetrominoSpawned?.Invoke(tetromino);
    } 

    private void OnDisable()
    {
        tetris.GameStarted -= SpawnNextTetromino;
        gridRowClearer.TetrominoCanSpawn -= SpawnNextTetromino;
        tetrominoHolder.TetrominoHeld -= OnTetrominoHeld;
    }

}
