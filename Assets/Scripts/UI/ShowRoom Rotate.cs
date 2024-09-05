// This script controls the rotation effect on the vehicle displayed in the background of the main menu

using UnityEngine;

public class ShowRoomRotate : MonoBehaviour
{
    // Variable to determine how fast the vehicle should rotate in the background
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        // Rotates the vehicle model
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
