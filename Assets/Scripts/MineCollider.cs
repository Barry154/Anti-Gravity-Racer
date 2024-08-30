using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;

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
        mineBody = gameObject.GetComponent<MeshRenderer>();
        mineCollider = gameObject.GetComponent<SphereCollider>();

        explosionSparks.Stop();
        explosionFlash.Stop();
        explosionFire.Stop();
        explosionSmoke.Stop();
    }

    IEnumerator ExplodeAndDestroy()
    {
        mineBody.enabled = false;
        mineCollider.enabled = false;

        explosionSparks.Play(true);
        explosionFlash.Play(true);
        explosionFire.Play(true);
        explosionSmoke.Play(true);

        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        //Destroy(gameObject);

        if (collision.gameObject.CompareTag("Missile"))
        {
            //Destroy(gameObject);
            GameManager.instance.targetsDestroyed += 1;
            StartCoroutine(ExplodeAndDestroy());
            //Debug.Log("Missile Hit");
        }

        else if (collision.gameObject.CompareTag("Vehicle"))
        {
            //Destroy(gameObject);
            StartCoroutine(ExplodeAndDestroy());
            //Debug.Log("Missile Hit");
        }
    }
}
