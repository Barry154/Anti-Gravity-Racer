using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThemeManager : MonoBehaviour
{
    [Header("Main Theme Audio Sources")]
    [SerializeField] AudioSource intro;
    [SerializeField] AudioSource main;

    public void PlayGameMusic()
    {
        main.Play();
    }

    public void PlayIntroMusic()
    {
        intro.Play();
    }
}
