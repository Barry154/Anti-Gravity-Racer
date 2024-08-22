using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
