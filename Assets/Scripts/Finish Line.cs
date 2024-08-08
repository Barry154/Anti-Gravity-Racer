using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [HideInInspector] public bool canLap;

    [SerializeField] public bool debugMode;

    private void Start()
    {
        canLap = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "vehicleSensor") { Debug.Log("Vehicle crossed finish line"); };

        if ((canLap || debugMode) && other.gameObject.CompareTag("vehicleSensor"))
        {
            GameManager.instance.LapCompleted();
            canLap = false;
        }
    }
}
