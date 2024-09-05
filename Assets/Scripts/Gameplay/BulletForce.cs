// Adds force to the weapon projectile (Missile)

using UnityEngine;

public class BulletForce : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float force;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component of the game object
        rb = GetComponent<Rigidbody>();

        // Add a force to the rigidbody in the positive z-axis (local) direction
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
