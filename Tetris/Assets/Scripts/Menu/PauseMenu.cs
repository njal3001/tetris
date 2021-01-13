using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{

    [SerializeField]
    private StartMenu startMenu;

    private void OnEnable() => tetrisState.OnPauseChanged += OnPauseChanged;

    public void Continue() => tetrisState.IsPaused = false;

    public void GoToStartMenu()
    {
        tetrisState.IsPaused = false;
        base.GoToMenu(startMenu);
    }

    private void OnPauseChanged(bool paused)
    {
        if (paused) base.Show();
        else base.Hide();
    }

    private void onDisable() => tetrisState.OnPauseChanged -= OnPauseChanged;

}
