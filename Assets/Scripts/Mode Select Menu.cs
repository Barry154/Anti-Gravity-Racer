using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    public void StartTimeAttack()
    {
        //SceneManager.LoadScene("Time Attack");
        levelLoader.StartSceneTransition(2);
    }

    public void StartTimeAttackLong()
    {
        //SceneManager.LoadScene("Time Attack Long");
        levelLoader.StartSceneTransition(3);
    }

    public void StartPilotsGauntlet()
    {
        //SceneManager.LoadScene("Pilots Gauntlet");
        levelLoader.StartSceneTransition(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
