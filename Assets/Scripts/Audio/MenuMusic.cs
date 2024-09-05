// This script manages the playback of the menu music

using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    // Declare this class as a static object
    public static MenuMusic menuMusic;

    // Prevent duplicates of the object from being created on object 'Awake'
    private void Awake()
    {
        // If the object already exists, destroy the duplicate
        if (menuMusic != null)
        {
            Destroy(gameObject);
        }
        // Otherwise, the object becomes 'this'
        else
        {
            menuMusic = this;
        }

        // Let the object persist between scene loads. This allows the music to play between the two scenes which start the game and present the player 
        // with the menus to choose a game mode
        DontDestroyOnLoad(this.gameObject);
    }

    // When this object needs to be destroyed (i.e when launching a game mode), call this function which makes the static object null, then destroys it.
    public void StopMenuMusic()
    {
        menuMusic = null;
        Destroy(gameObject);
    }
}
