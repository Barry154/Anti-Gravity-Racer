using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCheckpoint : MonoBehaviour
{
    [SerializeField] public FinishLine finishLine;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "vehicleSensor") { Debug.Log("vehicle entered checkpoint"); } 
        if (other.gameObject.CompareTag("vehicleSensor")) { finishLine.canLap = true; }
    }
}
