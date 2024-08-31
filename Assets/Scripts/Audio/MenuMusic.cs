using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic menuMusic;

    private void Awake()
    {
        if (menuMusic != null)
        {
            Destroy(gameObject);
        }
        else
        {
            menuMusic = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void StopMenuMusic()
    {
        menuMusic = null;
        Destroy(gameObject);
    }
}
