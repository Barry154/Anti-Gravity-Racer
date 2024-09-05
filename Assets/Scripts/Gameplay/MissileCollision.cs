// This script contols the collision interactions of the missile

using System.Collections;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    [Header("Mesh Renderer")]
    [SerializeField] MeshRenderer mesh;

    [Header("Box COllider")]
    [SerializeField] BoxCollider collider;

    [Header("Audio Source")]
    [SerializeField] AudioSource fireWeaponSound;

    // Called when another collider enters the collider of this game object
    private void OnCollisionEnter(Collision collision)
    {
        // Disable the mesh renderer of the game object (makes it invisible, but the game object is still active)
        mesh.enabled = false;
        // Disable the missile's collider
        collider.enabled = false;
        // Start the destruction process of the missile
        StartCoroutine(DestoryMissile(fireWeaponSound.clip.length));
    }

    // Coroutine which is called/started when the missile needs to be destroyed due to a collision
    IEnumerator DestoryMissile(float waitTime)
    {
        // Wait the amount of time it takes the 'fire weapon' SFX to finish playing
        yield return new WaitForSeconds(waitTime);
        // Destroy the missile game object
        Destroy(gameObject);
    }
}
