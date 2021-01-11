using UnityEngine;

public class TetrisAudio : MonoBehaviour
{

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private TetrisState tetrisState;

    [SerializeField]
    private TetrominoInput tetrominoInput;

    [SerializeField]
    private TetrominoHolder tetrominoHolder;

    [SerializeField]
    private TetrisGridRowClearer rowClearer;

    [SerializeField]
    private float tetrisSongFadeInTime;

    private void OnEnable()
    {
        tetrisState.OnGameStarted += PlayTetrisSong;
        tetrisState.OnTetrominoLockedInBounds += PlayTetrominoLock;
        tetrisState.OnGameOver += OnGameOver;

        tetrominoInput.TetrominoMoved += PlayTetrominoMove;

        tetrominoHolder.TetrominoHeld += PlayTetrominoHold;

        rowClearer.RowsCleared += PlayRowsCleared;
    }

    private void PlayTetrisSong() => audioManager.Play("tetrisSong", tetrisSongFadeInTime);
    private void PlayTetrominoMove() => audioManager.Play("tetrominoMove");
    private void PlayTetrominoHold(Tetromino tetromino) => audioManager.Play("tetrominoHold");
    private void PlayTetrominoLock() => audioManager.Play("tetrominoLock");

    private void PlayRowsCleared(int lines)
    {
        if (lines == 4) audioManager.Play("tetrisRowClear");
        else audioManager.Play("rowClear");
    }

    private void OnGameOver()
    {
        audioManager.Play("gameOver");
        audioManager.Stop("tetrisSong");
    }


    private void OnDisable()
    {
        tetrisState.OnGameStarted -= PlayTetrisSong;
        tetrisState.OnTetrominoLockedInBounds -= PlayTetrominoLock;
        tetrisState.OnGameOver -= OnGameOver;

        tetrominoInput.TetrominoMoved -= PlayTetrominoMove;

        tetrominoHolder.TetrominoHeld -= PlayTetrominoHold;

        rowClearer.RowsCleared -= PlayRowsCleared;
    }
}
