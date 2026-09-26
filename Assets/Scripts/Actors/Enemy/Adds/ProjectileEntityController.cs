using System;
using System.Collections;
using UnityEngine;

public class ProjectileEntityController : MonoBehaviour
{
    [SerializeField]
    protected float actionStartDelay = 4f;
    protected Projectile projectile;

    private static int projectileEntitesActive = 0;

    public static EventHandler<int> OnNewActiveEntities;

    protected virtual void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleEntityActive;
    }

    protected virtual void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleEntityActive;

        StopAllCoroutines();
    }

    protected virtual IEnumerator StartEntityAction()
    {
        yield return new WaitForSeconds(actionStartDelay);
    }

    protected virtual void ToggleEntityActive(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(StartEntityAction());
            projectileEntitesActive++;
        }
        else
        {
            StopAllCoroutines();
            projectileEntitesActive--;
        }

        OnNewActiveEntities?.Invoke(this, projectileEntitesActive);
        Debug.Log("Active Entities: " + projectileEntitesActive);
    }
}
