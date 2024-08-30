using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        //Destroy(gameObject);

        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(gameObject);
            GameManager.instance.targetsDestroyed += 1;
            //Debug.Log("Missile Hit");
        }
    }
}
