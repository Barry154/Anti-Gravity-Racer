using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        Destroy(gameObject);

        //if (collision.gameObject.CompareTag("Bullet"))
        //{
            
        //    Destroy(collision.gameObject);       
        //    GameManager.instance.targetsDestroyed += 1;
        //}
    }
}
