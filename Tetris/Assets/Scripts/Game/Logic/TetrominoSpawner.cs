using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{

    [SerializeField]
    private Vector2 spawnPoint;

    //Quick fix....
    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    private TetrominoGenerator tetrominoGenerator;

    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    [SerializeField]
    private TetrisGridRowClearer gridRowClearer;

    public event Action<Tetromino> TetrominoSpawned;

    private void OnEnable()
    {
        gridRowClearer.TetrominoCanSpawn += SpawnNextTetromino;
        tetrominoHolder.TetrominoChanged += OnTetrominoSwapped;
    }

    public void SpawnNextTetromino() => StartCoroutine(SpawnTetromino(tetrominoGenerator.NextTetromino));
 
    private void OnTetrominoSwapped(Tetromino prevHolding, Tetromino holding)
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
        gridRowClearer.TetrominoCanSpawn -= SpawnNextTetromino;
        tetrominoHolder.TetrominoChanged -= OnTetrominoSwapped;
    }

}
