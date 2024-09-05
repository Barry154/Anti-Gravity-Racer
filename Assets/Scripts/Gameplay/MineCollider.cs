// This script controls the collision interactions of the mines

using System.Collections;
using UnityEngine;

public class MineCollider : MonoBehaviour
{
    [Header("Particle Effect (Explosion)")]
    [SerializeField] ParticleSystem explosionSparks;
    [SerializeField] ParticleSystem explosionFlash;
    [SerializeField] ParticleSystem explosionFire;
    [SerializeField] ParticleSystem explosionSmoke;
    [SerializeField] float waitTime = 1;

    MeshRenderer mineBody;
    SphereCollider mineCollider;

    private void Start()
    {
        // Get the mesh render of the game object
        mineBody = gameObject.GetComponent<MeshRenderer>();
        // Get the sphere collider of the game object
        mineCollider = gameObject.GetComponent<SphereCollider>();

        // Stop playback of all the particle effects which make up the explosion VFX
        explosionSparks.Stop();
        explosionFlash.Stop();
        explosionFire.Stop();
        explosionSmoke.Stop();
    }

    // Coroutine which starts when the mine should be destroyed (shot with a missile or hit by the vehicle)
    IEnumerator ExplodeAndDestroy()
    {
        // Play the explosion SFX
        GameManager.instance.sfxManager.PlayExplosionSFX();

        // Disable the mesh renderer (makes it invisible, but the game object is still active))
        mineBody.enabled = false;
        // Disable the mine's collider
        mineCollider.enabled = false;

        // Play the explosion VFX particle effect systems
        explosionSparks.Play(true);
        explosionFlash.Play(true);
        explosionFire.Play(true);
        explosionSmoke.Play(true);

        // Wait until the particle systems have finished playing
        yield return new WaitForSeconds(waitTime);

        // Destroy the game object
        Destroy(gameObject);
    }

    // Called when another collider enters the collider of this game object
    private void OnCollisionEnter(Collision collision)
    {
        // If the colliding object is a missile
        if (collision.gameObject.CompareTag("Missile"))
        {
            // Increase targets destroyed score
            GameManager.instance.targetsDestroyed += 1;
            // Start the destruction process of the mine
            StartCoroutine(ExplodeAndDestroy());
        }

        // If the colliding object is the vehicle
        else if (collision.gameObject.CompareTag("Vehicle"))
        {
            // Start the destruction process of the mine
            StartCoroutine(ExplodeAndDestroy());
        }
    }
}
