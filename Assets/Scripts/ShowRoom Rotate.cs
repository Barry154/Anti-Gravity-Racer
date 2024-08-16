using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoomRotate : MonoBehaviour
{
    // Variable to determine how fast the vehicle should rotate in the background
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
