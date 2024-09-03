using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private TextMeshProUGUI credits;

    public void StartGame()
    {
        levelLoader.StartSceneTransition(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeColorPointerEnter()
    {
        credits.color = Color.green;
    }

    public void ChangeColorPointerExit()
    {
        credits.color = Color.yellow;
    }
}
