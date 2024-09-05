// Destroys the object after a defined amount of time

using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    // The amount of time to delay the destroy function call
    [SerializeField] float seconds;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, seconds);
    }
}
