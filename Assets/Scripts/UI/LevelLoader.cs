// This script controls the scene transition animations

// Code and technique was learnt from a YouTube tutorial by Brackeys and amended to fit within the context of this project
// Source: https://www.youtube.com/watch?v=CE9VOZivb3I

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] public Animator transition;
    [SerializeField] public float transitionTime = 1f;

    // This function can be called outside this class when a scene transition needs to occur
    public void StartSceneTransition(int sceneIndex)
    {
        // Starts the coroutine which plays the scene transition animation
        StartCoroutine(SceneTransition(sceneIndex));
    }

    // Plays the scene transition animation, then loads the scene based on the index parsed as an argument
    IEnumerator SceneTransition(int sceneIndex)
    {
        // Set animation trigger
        transition.SetTrigger("Start");

        // wait the duration of the animation
        yield return new WaitForSeconds(transitionTime);

        // Load the scene based on its index
        SceneManager.LoadScene(sceneIndex);
    }
}
