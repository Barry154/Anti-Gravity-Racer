// This script manages the playback of the in-game music

using UnityEngine;

public class MainThemeManager : MonoBehaviour
{
    [Header("Main Theme Audio Sources")]
    [SerializeField] AudioSource intro;
    [SerializeField] AudioSource main;

    // Plays the main musical loop for the game
    public void PlayGameMusic()
    {
        main.Play();
    }

    // Plays the intro for the main musical loop
    public void PlayIntroMusic()
    {
        intro.Play();
    }
}
