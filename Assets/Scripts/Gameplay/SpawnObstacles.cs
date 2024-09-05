// This script controls the activation of obstacle game obejects for the Pilot's Gauntlet

using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [Header("Obstacle Game Object Containers")]
    [SerializeField] GameObject mineField1;
    [SerializeField] GameObject mineField2;
    [SerializeField] GameObject mineField3;
    [SerializeField] GameObject pillars;
    [SerializeField] GameObject walls;

    [HideInInspector] public int numMinesField1;
    [HideInInspector] public int numMinesField2;
    [HideInInspector] public int numMinesField3;
    [HideInInspector] public int totalMineCount;

    private void Start()
    {
        // The child counts of the mine container game objects are used for stat checking post game so that the player
        // may see how many mines, out of the total per lap, they were able to shoot
        numMinesField1 = mineField1.transform.childCount;
        numMinesField2 = mineField2.transform.childCount;
        numMinesField3 = mineField3.transform.childCount;
        totalMineCount = numMinesField1 + numMinesField2 + numMinesField3;
    }

    // Spawns (activates the container game object) the first set of mines
    public void SpawnMines()
    {
        // Calls the SFX manager from the Game Manager to play the warning SFX
        GameManager.instance.sfxManager.PlayWarningPromptSFX();

        // Activate the first set of mines
        mineField1.SetActive(true);
    }

    // Spawns (activates the container game object) the wall obstacles
    public void SpawnWalls()
    {
        // Calls the SFX manager from the Game Manager to play the warning SFX
        GameManager.instance.sfxManager.PlayWarningPromptSFX();

        // Deactivates the container game object of the first set of mines
        mineField1.SetActive(false);
        // Spawns (activates the container game object) the second set of mines
        mineField2.SetActive(true);
        // Activate the wall obstacles
        walls.SetActive(true);
    }

    // Spawns (activates the container game object) the pillar/cylinder obstacles
    public void SpawnPillars()
    {
        // Calls the SFX manager from the Game Manager to play the warning SFX
        GameManager.instance.sfxManager.PlayWarningPromptSFX();

        // Deactivates the container game object of the second set of mines
        mineField2.SetActive(false);
        // Spawns (activates the container game object) the third set of mines
        mineField3.SetActive(true);
        // Activate the pillar/cylinder obstacles
        pillars.SetActive(true);
    }

    
}
