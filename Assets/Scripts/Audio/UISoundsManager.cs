// This script manages the playback of the UI sound effects

using UnityEngine;

public class UISoundsManager : MonoBehaviour
{
    [Header("UI Audio Sources")]
    [SerializeField] AudioSource hover;
    [SerializeField] AudioSource clicked;
    [SerializeField] AudioSource loadScene;

    // Plays the SFX for when the mouse pointer enters the UI element
    public void PlayHoverSound()
    {
        hover.Play();
    }

    // Plays the SFX for when the UI element (button) is clicked (called in the button's onClick events)
    public void PlayClickedSound()
    {
        clicked.Play();
    }

    // Plays the SFX for when a button click loads a new scene (called in the button's onClick events)
    public void PlayLoadSceneSound()
    {
        loadScene.Play();
    }
}
