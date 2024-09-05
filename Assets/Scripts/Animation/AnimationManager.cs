// This script controls the in game animation triggers for warning canvases and pilot gauntlet obstacle spawns

using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("UI Animation Controllers")]
    [SerializeField] Animator hullStrengthWarning;
    [SerializeField] Animator minesWarning;
    [SerializeField] Animator wallsWarning;
    [SerializeField] Animator pillarsWarning;

    [Header("Obstacle Animation Controllers")]
    [SerializeField] Animator spawnWall;
    [SerializeField] Animator spawnPillar;

    // The booleans are used to be sure the animations for the warning canvases only play once
    [Header("UI Animation Management Booleans")]
    public bool hullWarningPlayed = false;
    public bool mineWarningPlayed = false;
    public bool wallWarningPlayed = false;
    public bool pillarWarningPlayed = false;

    [Header("Obstacle Animation Management Booleans")]
    public bool wallSpawnPlayed = false;
    public bool pillarSpawnPlayed = false;

    // Displays the 'hull warning' HUD when durability is low. Makes the text blink.
    public void StartHullWarningBlink()
    {
        hullStrengthWarning.SetTrigger("StartBlink");
        hullWarningPlayed = true;
    }

    // Displays the warning text that tells the players mines are spawning. Makes the text blink.
    public void StartMineWarning()
    {
        minesWarning.SetTrigger("StartBlink");
        mineWarningPlayed = true;
    }

    // Starts the animation for the 1st wall of the wall obstacles to make it appear as it is shifting into the world,
    // instead of just appearing. Also makes the obstacle warning canvas start blinking to inform the player. 
    public void SpawnWallAnimation()
    {
        wallsWarning.SetTrigger("StartBlink");
        wallWarningPlayed = true;

        spawnWall.SetTrigger("Spawn");
        wallSpawnPlayed = true;
    }

    // Starts the animation for the 1st pillar of the pillar obstacles to make it appear as it is shifting into the world,
    // instead of just appearing. Also makes the obstacle warning canvas start blinking to inform the player. 
    public void SpawnPillarAnimation()
    {
        pillarsWarning.SetTrigger("StartBlink");
        pillarWarningPlayed = true;

        spawnPillar.SetTrigger("Spawn");
        pillarSpawnPlayed = true;
    }
}
