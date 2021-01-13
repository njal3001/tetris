using UnityEngine;

public class StartMenu : Menu
{
    [SerializeField]
    private GameObject playingDisplay;

    private void Start() => tetrisState.Clear();

    public override void Show()
    {
        tetrisState.Clear();
        base.Show();
        SetActive(false, playingDisplay);
    }

    public override void Hide()
    {
        base.Hide();
        SetActive(true, playingDisplay);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
