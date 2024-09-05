// This script manages the playback of the in-game sound effects

using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Vehicle & Weapon Sound Effects")]
    [SerializeField] AudioSource boostSFX;
    [SerializeField] AudioSource explosion;
    [SerializeField] AudioSource collision;
    [SerializeField] AudioSource wallgrind;

    [Header("Gameplay Sound Effects")]
    [SerializeField] AudioSource lapComplete;
    [SerializeField] AudioSource gameComplete;
    [SerializeField] AudioSource gameFailed;
    [SerializeField] AudioSource warningPrompt;
    [SerializeField] AudioSource dangerAlert;

    // Danger SFX 'already played' boolean to prevent multiple playback calls.
    bool dangerAlertPlayed = false;

    // Plays the SFX when the player uses the boost control
    public void PlayBoostSFX()
    {
        boostSFX.Play();
    }

    // Plays the SFX for when either a mine or the vehicle itself is destroyed
    public void PlayExplosionSFX()
    {
        explosion.Play();
    }

    // Plays the SFX for when the vehicle makes contact with another object within the game world
    public void PlayCollisionSFX()
    {
        if (!collision.isPlaying)
        {
            collision.Play();
        }
    }

    // Plays the SFX when the vehicle maintains contact with a wall
    public void PlayWallgrindSFX()
    {
        if (!wallgrind.isPlaying)
        {
            wallgrind.Play();
        } 
    }

    // Stops the continuous wall contact SFX when contact with the wall stops
    public void StopWallgrindSFX()
    {
        wallgrind.Stop();
    }

    // PLays the SFX for when the vehicle crosses the finish line
    public void PlayLapCompleteSFX()
    {
        lapComplete.Play();
    }

    // Plays the SFX for when the player completes a game mode
    public void PlayGameCompleteSFX()
    {
        gameComplete.Play();
    }

    // Plays the SFX for when the player fails the game objective (falling off the track or destroying the vehicle)
    public void PlayGameFailedSFX()
    {
        gameFailed.Play();
    }

    // Plays the SFX for when the warning prompt canvases display
    public void PlayWarningPromptSFX()
    {
        warningPrompt.Play();
    }

    // Plays the SFX for when the vehicle durability reaches the 'critical' value
    // Also sets the boolean which tracks the SFX playback to true
    public void PlayDangerAlertSFX()
    {
        if (!dangerAlertPlayed)
        {
            dangerAlert.Play();
            dangerAlertPlayed = true;
        }
    }

}
