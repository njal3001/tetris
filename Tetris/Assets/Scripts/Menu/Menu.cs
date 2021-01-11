using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField]
    protected TetrisState tetrisState;

    public virtual void PlayGame()
    {
        Hide();
        tetrisState.StartGame();
    }

    public virtual void Show() => SetActive(true);

    public virtual void Hide() => SetActive(false);

    protected void SetActive(bool active) => SetActive(active, transform.GetChild(0).gameObject);

    protected static void SetActive(bool active, GameObject gameObject) => gameObject.SetActive(active);
}
