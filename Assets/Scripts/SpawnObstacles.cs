using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [Header("Obstacle Game Object Containers")]
    [SerializeField] GameObject mines;
    [SerializeField] GameObject pillars;
    [SerializeField] GameObject walls;

    public void SpawnMines()
    {
        Debug.Log("Spawn Mines");

        mines.SetActive(true);
    }

    public void SpawnPillars()
    {
        Debug.Log("Spawn Pillars");

        pillars.SetActive(true);
    }

    public void SpawnWalls()
    {
        Debug.Log("Spawn Walls");

        walls.SetActive(true);
    }
}
