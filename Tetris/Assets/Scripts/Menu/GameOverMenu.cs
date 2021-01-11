using System.Collections;
using System.Collections.Generic;
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

    public void GoToMenu()
    {
        base.Hide();
        startMenu.Show();
    }

    private void OnDisable() => tetrisState.OnGameOver -= base.Show;
}
