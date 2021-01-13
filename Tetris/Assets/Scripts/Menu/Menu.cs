using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField]
    protected TetrisState tetrisState;

    [SerializeField]
    private GameObject backgroundTint;

    public virtual void PlayGame()
    {
        Hide();
        tetrisState.StartGame();
    }

    public virtual void Show()
    {
        SetActive(true);
        SetActive(true, backgroundTint);
    }

    public virtual void Hide()
    {
        SetActive(false);
        SetActive(false, backgroundTint);
    }

    protected void SetActive(bool active) => SetActive(active, transform.GetChild(0).gameObject);

    protected void GoToMenu(Menu menu)
    {
        Hide();
        menu.Show();
    }

    protected static void SetActive(bool active, GameObject gameObject) => gameObject.SetActive(active);
}
