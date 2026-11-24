using System;
using System.Collections;
using UnityEngine;

public class ProjectileEntityController : MonoBehaviour
{
    protected bool entityActive = false;
    protected string attackSignature;

    [SerializeField]
    protected float actionStartDelay = 4f;

    [SerializeField]
    protected VFXPlayer entitySpawnVFX;

    // [SerializeField]
    // protected VFXPlayer entityDestroyedVFX;
    protected Projectile projectile;

    private static int projectileEntitiesActive = 0;

    public static EventHandler<int> OnNewActiveEntities;

    protected virtual void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleEntityActive;
        projectile.OnSetNewSignature += UpdateSignature;
    }

    protected virtual void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleEntityActive;
        projectile.OnSetNewSignature -= UpdateSignature;

        StopAllCoroutines();
    }

    protected virtual IEnumerator StartEntityAction()
    {
        yield return new WaitForSeconds(actionStartDelay);
    }

    protected virtual void StopEntityAction()
    {
        StopAllCoroutines();
    }

    protected virtual void ToggleEntityActive(object sender, bool toggle)
    {
        if (toggle)
        {
            if (entitySpawnVFX)
            {
                entitySpawnVFX.PlayVFX(transform);
            }

            StartCoroutine(StartEntityAction());
        }
        else
        {
            // if (entityDestroyedVFX)
            // {
            //     entityDestroyedVFX.PlayVFX(transform);
            // }

            StopEntityAction();
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

    private void UpdateSignature(object sender, string attackSignature)
    {
        this.attackSignature = attackSignature;
    }
}
