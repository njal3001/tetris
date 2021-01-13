using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrisState : MonoBehaviour
{

    [SerializeField]
    private List<MonoBehaviour> pauseScripts = new List<MonoBehaviour>();
    private bool isPaused;

    public event Action OnGameStarted;
    public event Action OnClear;
    public event Action OnTetrominoLocked;
    public event Action OnTetrominoLockedInBounds;
    public event Action<bool> OnPauseChanged;
    public event Action OnGameOver;

    public void StartGame()
    {
        OnGameStarted?.Invoke();
        IsPlaying = true;
    }

    public void Clear() => OnClear?.Invoke();

    public void TetrominoLocked(Tetromino tetromino)
    {
        OnTetrominoLocked?.Invoke();

        if (tetromino.OutOfSight())
        {
            OnGameOver?.Invoke();
            IsPlaying = false;
        }
        else
            OnTetrominoLockedInBounds?.Invoke();
    }

    private bool IsPlaying;
 
    public bool IsPaused
    {
        get => isPaused;
        set
        {
            if (isPaused == value) return;

            foreach (MonoBehaviour pauseScript in pauseScripts)
                pauseScript.enabled = !value;

            isPaused = value;
            OnPauseChanged?.Invoke(value);
        }
    }

    private void Update()
    {
        if (IsPlaying && Input.GetKeyDown(KeyCode.Escape)) 
            IsPaused = !IsPaused;
    }

}
