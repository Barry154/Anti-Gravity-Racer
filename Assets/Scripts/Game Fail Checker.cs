using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFailChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("vehicleSensor"))
        {
            GameManager.instance.FellOffTrack();
        }
    }
}
