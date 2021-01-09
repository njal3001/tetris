using UnityEngine;

public class TetrisAudio : MonoBehaviour
{
    //Subject to change, might just remove the audioManager class...

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private TetrisState tetrisState;

    [SerializeField]
    private TetrominoInput tetrominoInput;

    [SerializeField]
    private TetrisGridRowClearer rowClearer;

    private void OnEnable()
    {
        tetrisState.GameStarted += PlayTetrisSong;
        tetrisState.TetrominoLockedInBounds += PlayTetrominoLock;
        tetrisState.GameOver += PlayGameOver;

        tetrominoInput.TetrominoMoved += PlayTetrominoMove;

        rowClearer.RowsCleared += PlayRowsCleared;
    }

    private void PlayTetrisSong() => audioManager.Play("tetrisSong");
    private void PlayTetrominoMove() => audioManager.Play("tetrominoMove");
    private void PlayTetrominoLock() => audioManager.Play("tetrominoLock");

    private void PlayRowsCleared(int lines)
    {
        if (lines == 4) audioManager.Play("tetrisRowClear");
        else audioManager.Play("rowClear");
    }

    private void PlayGameOver() => audioManager.Play("gameOver");


    private void OnDisable()
    {
        tetrisState.GameStarted -= PlayTetrisSong;
        tetrisState.TetrominoLockedInBounds -= PlayTetrominoLock;
        tetrisState.GameOver -= PlayGameOver;

        tetrominoInput.TetrominoMoved -= PlayTetrominoMove;

        rowClearer.RowsCleared -= PlayRowsCleared;
    }
}
