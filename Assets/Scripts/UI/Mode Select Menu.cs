using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    public void StartTimeAttack()
    {
        MenuMusic.menuMusic.StopMenuMusic();
        levelLoader.StartSceneTransition(2);
    }

    public void StartTimeAttackLong()
    {
        MenuMusic.menuMusic.StopMenuMusic();
        levelLoader.StartSceneTransition(3);
    }

    public void StartPilotsGauntlet()
    {
        MenuMusic.menuMusic.StopMenuMusic();
        levelLoader.StartSceneTransition(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
