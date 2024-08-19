using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBullet : MonoBehaviour
{
    [SerializeField] public GameObject bullet;
    [SerializeField] PlayerInput playerInput;

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fireWeapon)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        } 
    }
}
