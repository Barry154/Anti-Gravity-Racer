// This script controls the start screen interactions, mainly transitioning to the game's main menu, or quitting the application (in standalone builds)

using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private TextMeshProUGUI credits;

    // Load the main menu scene by index
    public void StartGame()
    {
        levelLoader.StartSceneTransition(1);
    }

    // Quits the game/application in standalone builds
    public void QuitGame()
    {
        Application.Quit();
    }

    // Changes the colour of the text of the 'credits' button on pointer enter
    public void ChangeColorPointerEnter()
    {
        credits.color = Color.green;
    }

    // Changes the colour of the text of the 'credits' button on pointer exit
    public void ChangeColorPointerExit()
    {
        credits.color = Color.yellow;
    }
}
