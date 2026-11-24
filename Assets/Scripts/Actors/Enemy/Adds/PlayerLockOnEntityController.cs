using System.Collections;
using UnityEngine;

public class PlayerLockOnEntityController : ProjectileEntityController
{
    private bool followPlayer = false;
    private Transform playerTransform;

    [SerializeField]
    protected ProjectileMover projectileMover;

    protected private void Update()
    {
        if (followPlayer)
        {
            FollowPlayerPosition();
        }
    }

    protected override IEnumerator StartEntityAction()
    {
        ToggleFollowPlayer(true);

        yield return base.StartEntityAction();

        ToggleFollowPlayer(false);
    }

    protected void ToggleFollowPlayer(bool toggle)
    {
        if (!playerTransform)
        {
            playerTransform = PlayerIdentifier.PlayerTransform;
        }

        followPlayer = toggle;
    }

    protected void FollowPlayerPosition()
    {
        if (!playerTransform)
        {
            return;
        }

        projectileMover.SetProjectilePosition(playerTransform.position);
    }
}
