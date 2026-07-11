using System;
using System.Collections;
using UnityEngine;

public class MeleeCrowController : MonoBehaviour
{
    [SerializeField]
    private float movementStartDelay = 4f;

    [SerializeField]
    private float movementSpeed = 6f;

    [SerializeField]
    private int impactDamage = 5;

    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleCrowActive;
    }

    private void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleCrowActive;
    }

    private IEnumerator StartCrowMovement()
    {
        yield return new WaitForSeconds(movementStartDelay);

        projectile.SetSpeedAndDamage(movementSpeed, impactDamage);
    }

    private void ToggleCrowActive(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(StartCrowMovement());
        }
        else
        {
            StopAllCoroutines();
        }
    }
}
