using System;
using UnityEngine;

public class FellFlashController : RedFlashController
{
    [SerializeField]
    private bool isRightWing = false;
    private float fellFlashModifier = 2f;

    private void Awake()
    {
        flashDuration = flashDuration * fellFlashModifier;
    }

    protected override void OnEnable()
    {
        BossFellWingState.OnWingTelegraph += TryPerformFlash;
    }

    protected override void OnDisable()
    {
        BossFellWingState.OnWingTelegraph -= TryPerformFlash;
    }

    private void TryPerformFlash(object sender, int e)
    {
        if ((!isRightWing && (e <= 0f)) || (isRightWing && (e >= 1f)))
        {
            PerformFlash();
        }
    }
}
