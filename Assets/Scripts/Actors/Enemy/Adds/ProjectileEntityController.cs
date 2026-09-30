using System;
using System.Collections;
using UnityEngine;

public class ProjectileEntityController : MonoBehaviour
{
    protected bool entityActive = false;

    [SerializeField]
    protected float actionStartDelay = 4f;
    protected Projectile projectile;

    private static int projectileEntitiesActive = 0;

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
        }
        else
        {
            StopAllCoroutines();
        }

        if (toggle == entityActive)
        {
            return;
        }

        entityActive = toggle;

        if (toggle)
        {
            projectileEntitiesActive++;
        }
        else
        {
            projectileEntitiesActive--;
        }

        OnNewActiveEntities?.Invoke(this, projectileEntitiesActive);
        Debug.Log("Active Entities: " + projectileEntitiesActive);
    }
}
