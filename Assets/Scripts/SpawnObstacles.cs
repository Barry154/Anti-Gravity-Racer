using System.Collections;
using System.Collections.Generic;
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
        numMinesField1 = mineField1.transform.childCount;
        numMinesField2 = mineField2.transform.childCount;
        numMinesField3 = mineField3.transform.childCount;
        totalMineCount = numMinesField1 + numMinesField2 + numMinesField3;

        //Debug.Log(numMinesField1);
        //Debug.Log(numMinesField2);
        //Debug.Log(numMinesField3);
        //Debug.Log(totalMineCount);
    }

    public void SpawnMines()
    {
        //Debug.Log("Spawn Mines");

        mineField1.SetActive(true);
    }

    public void SpawnWalls()
    {
        //Debug.Log("Spawn Walls");

        mineField1.SetActive(false);
        mineField2.SetActive(true);
        walls.SetActive(true);
    }

    public void SpawnPillars()
    {
        //Debug.Log("Spawn Pillars");

        mineField2.SetActive(false);
        mineField3.SetActive(true);
        pillars.SetActive(true);
    }

    
}
