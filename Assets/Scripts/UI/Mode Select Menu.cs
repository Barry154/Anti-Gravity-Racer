// This script controls the scene transition management from the main menu so that the player may load a game mode

using UnityEngine;

public class ModeSelectMenu : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    // Loads the Time Attack game mode scene
    public void StartTimeAttack()
    {
        // Destroys the object which controls the menu music
        MenuMusic.menuMusic.StopMenuMusic();
        // Load the Time Attack scene by index
        levelLoader.StartSceneTransition(2);
    }

    // Loads the Time Attack game mode scene (long track version)
    public void StartTimeAttackLong()
    {
        // Destroys the object which controls the menu music
        MenuMusic.menuMusic.StopMenuMusic();
        // Load the Time Attack scene by index (long track version)
        levelLoader.StartSceneTransition(3);
    }

    // Loads the Pilot's Gauntlet game mode scene
    public void StartPilotsGauntlet()
    {
        // Destroys the object which controls the menu music
        MenuMusic.menuMusic.StopMenuMusic();
        // Load the Pilot's Gauntlet scene by index
        levelLoader.StartSceneTransition(4);
    }

    // Quits the game/application in standalone builds
    public void QuitGame()
    {
        Application.Quit();
    }
}
