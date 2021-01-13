using UnityEngine;

public class GameOverMenu : Menu
{

    [SerializeField]
    private StartMenu startMenu;

    private void OnEnable() => tetrisState.OnGameOver += base.Show;

    public override void PlayGame()
    {
        tetrisState.Clear();
        base.PlayGame();
    }

    public void GoToStartMenu() => base.GoToMenu(startMenu);

    private void OnDisable() => tetrisState.OnGameOver -= base.Show;
}
