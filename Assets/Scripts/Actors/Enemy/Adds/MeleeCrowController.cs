using System.Collections;
using UnityEngine;

public class MeleeCrowController : ProjectileEntityController
{
    [SerializeField]
    private float movementSpeed = 6f;

    [SerializeField]
    private int impactDamage = 5;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        projectile.SetSpeedAndDamage(movementSpeed, impactDamage);
    }
}
