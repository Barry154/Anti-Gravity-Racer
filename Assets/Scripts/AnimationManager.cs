using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Animation Controllers")]
    [SerializeField] Animator hullStrengthWarning;

    [Header("Animation Management Booleans")]
    // Animation management booleans
    public bool hullWarningPlayed = false;
    public bool mineWarningPlayed = false;
    public bool obstacleWarning1_Played = false;
    public bool obstacleWarning2_Played = false;

    public void StartHullWarningBlink()
    {
        hullStrengthWarning.SetTrigger("StartBlink");
        hullWarningPlayed = true;
    }
}
