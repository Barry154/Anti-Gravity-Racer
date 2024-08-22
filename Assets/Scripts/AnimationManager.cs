using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Animation Controllers")]
    [SerializeField] Animator hullStrengthWarning;

    // Animation management booleans
    public bool hullWarningPlayed = false;

    public void StartBlink()
    {
        hullStrengthWarning.SetTrigger("StartBlink");
        hullWarningPlayed = true;
    }

    public void EndBlink()
    {
        hullStrengthWarning.SetTrigger("EndBlink");
    }
}
