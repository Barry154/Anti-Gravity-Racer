// Code from YouTube tutorial by Radial Games
// Source: https://www.youtube.com/watch?v=r9OEZmbD9q0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackObject : MonoBehaviour
{
    public Transform target;
    public float distanceUP;
    public float distanceBACK;
    public float minHeight;

    // Not used directly
    private Vector3 posVelocity;

    private void FixedUpdate()
    {
        // Calculate a new position to place the camera
        Vector3 newPos = target.position + (target.forward * distanceBACK);
        newPos.y = Mathf.Max(newPos.y + distanceUP, minHeight);

        // Move camera
        transform.position = newPos;
        //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref posVelocity, 0.18f);

        // Rotate the camera to keep the vehicle in view
        Vector3 focalPoint = target.position + (target.forward * 5);
        transform.LookAt(focalPoint);
    }
}
