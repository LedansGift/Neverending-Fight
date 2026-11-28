using System.Collections;
using UnityEngine;

public class MagpieKeepawayInterrupt : BossInterrupt
{
    private bool interruptAvailable = true;
    private float interruptCooldown = 30f;
    private float closeDistanceTime = 0f;
    private float closeDistanceTimeInterruptTrigger = 5f;

    private const float CLOSE_DISTANCE_THRESHOLD = 6f;
    private Transform playerTransform;

    private void Update()
    {
        if (interruptActive)
        {
            CalculatePlayerDistance();
        }
    }

    private IEnumerator InterruptCooldown()
    {
        interruptAvailable = false;
        yield return new WaitForSeconds(interruptCooldown);
        interruptAvailable = true;
    }

    private void CalculatePlayerDistance()
    {
        if (!playerTransform)
        {
            return;
        }

        float playerBossDistance = Vector3.Distance(playerTransform.position, transform.position);

        if (playerBossDistance <= CLOSE_DISTANCE_THRESHOLD)
        {
            closeDistanceTime += Time.deltaTime;
        }
        else
        {
            closeDistanceTime = 0f;
        }
    }

    public override void ResetInterrupt()
    {
        StopAllCoroutines();
        interruptAvailable = true;
    }

    public override bool ResolveInterrupt(out BossInterruptPhase outInterruptPhase)
    {
        outInterruptPhase = bossInterruptPhase;

        if (interruptAvailable && (closeDistanceTime > closeDistanceTimeInterruptTrigger))
        {
            StartCoroutine(InterruptCooldown());
            return true;
        }

        return false;
    }

    public override void SaveInterrupt()
    {
        StopAllCoroutines();
        interruptAvailable = true;
    }

    protected override void ActivateInterrupt()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
    }

    protected override void DeactivateInterrupt() { }
}
