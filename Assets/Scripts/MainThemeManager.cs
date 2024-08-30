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
        intro.Play();
        main.PlayDelayed(intro.clip.length);
    }
}
