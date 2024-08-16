using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    public void StartTimeAttack()
    {
        SceneManager.LoadScene("Time Attack");
    }

    public void StartTimeAttackLong()
    {
        SceneManager.LoadScene("Time Attack Long");
    }

    public void StartPilotsGauntlet()
    {
        SceneManager.LoadScene("Pilots Gauntlet");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
