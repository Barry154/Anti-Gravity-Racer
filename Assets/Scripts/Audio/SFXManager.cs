using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Vehicle & Weapon Sound Effects")]
    [SerializeField] AudioSource boostSFX;
    [SerializeField] AudioSource fireWeapon;
    [SerializeField] AudioSource explosion;
    [SerializeField] AudioSource collision;
    [SerializeField] AudioSource wallgrind;

    [Header("Gameplay Sound Effects")]
    [SerializeField] AudioSource lapComplete;
    [SerializeField] AudioSource gameComplete;
    [SerializeField] AudioSource gameFailed;
    [SerializeField] AudioSource warningPrompt;
    [SerializeField] AudioSource dangerAlert;

    // Danger SFX 'already played' booleans
    bool dangerAlertPlayed = false;

    public void PlayBoostSFX()
    {
        boostSFX.Play();
    }

    public void PlayFireWeaponSFX()
    {
        fireWeapon.Play();
    }

    public void PlayExplosionSFX()
    {
        explosion.Play();
    }

    public void PlayCollisionSFX()
    {
        if (!collision.isPlaying)
        {
            collision.Play();
        }
    }

    public void PlayWallgrindSFX()
    {
        if (!wallgrind.isPlaying)
        {
            wallgrind.Play();
        } 
    }

    public void StopWallgrindSFX()
    {
        wallgrind.Stop();
    }

    public void PlayLapCompleteSFX()
    {
        lapComplete.Play();
    }

    public void PlayGameCompleteSFX()
    {
        gameComplete.Play();
    }

    public void PlayGameFailedSFX()
    {
        gameFailed.Play();
    }

    public void PlayWarningPromptSFX()
    {
        warningPrompt.Play();
    }

    public void PlayDangerAlertSFX()
    {
        if (!dangerAlertPlayed)
        {
            dangerAlert.Play();
            dangerAlertPlayed = true;
        }
    }

}
