using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundsManager : MonoBehaviour
{
    [Header("UI Audio Sources")]
    [SerializeField] AudioSource hover;
    [SerializeField] AudioSource clicked;
    [SerializeField] AudioSource loadScene;

    public void PlayHoverSound()
    {
        hover.Play();
    }

    public void PlayClickedSound()
    {
        clicked.Play();
    }

    public void PlayLoadSceneSound()
    {
        loadScene.Play();
    }
}
