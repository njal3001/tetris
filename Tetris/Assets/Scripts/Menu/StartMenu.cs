using UnityEngine;

public class StartMenu : MonoBehaviour
{

    [SerializeField]
    private TetrisState tetrisState;

    public void PlayGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        tetrisState.StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
