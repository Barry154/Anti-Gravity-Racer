using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    [Header("Mesh Renderer")]
    [SerializeField] MeshRenderer mesh;

    [Header("Box COllider")]
    [SerializeField] BoxCollider collider;

    [Header("Audio Source")]
    [SerializeField] AudioSource fireWeaponSound;

    private void OnCollisionEnter(Collision collision)
    {
        mesh.enabled = false;
        collider.enabled = false;
        StartCoroutine(DestoryMissile(fireWeaponSound.clip.length));
    }

    IEnumerator DestoryMissile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
