using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    public void StartGame()
    {
        levelLoader.StartSceneTransition(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
