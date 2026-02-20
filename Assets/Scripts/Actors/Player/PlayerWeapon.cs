using System;
using UnityEngine;

public struct WeaponAbilityCharge
{
    public WeaponAbilityCharge(int weaponIndex, float weaponCharge)
    {
        this.weaponIndex = weaponIndex;
        this.weaponCharge = weaponCharge;
    }

    public int weaponIndex;
    public float weaponCharge;
}

public abstract class PlayerWeapon : MonoBehaviour
{
    private bool weaponActive = false;

    protected bool isBusy = false;
    protected bool canSwap = true;
    protected LayerMask attackLayerMask;
    protected Transform mouseTarget;

    [SerializeField]
    protected PlayerStats playerStats;

    [SerializeField]
    protected PlayerMovement playerMovement;

    [SerializeField]
    protected Animator weaponAnimator;

    public static EventHandler<WeaponAbilityCharge> OnWeaponAbilityCharge;

    public abstract void WeaponAttackStart();
    public abstract void WeaponAttackEnd();

    public abstract void WeaponSpecial();

    public void SetAttackLayerMask(int layermask)
    {
        attackLayerMask = layermask;
    }

    public virtual void ActivateWeapon()
    {
        if (weaponActive)
        {
            return;
        }

        weaponAnimator.SetTrigger("draw");
        isBusy = false;
        canSwap = true;
        playerMovement.SetWeaponModifier();

        weaponActive = true;
    }

    public virtual void StowWeapon()
    {
        if (!weaponActive)
        {
            return;
        }

        weaponAnimator.SetTrigger("stow");
        isBusy = true;
        canSwap = false;

        weaponActive = false;
    }

    public void SetMouseTarget(Transform targetTransform)
    {
        mouseTarget = targetTransform;
    }

    public bool CanSwap()
    {
        return canSwap;
    }

    public abstract void ResetWeapon();

    public abstract int GetWeaponIndex();

    //have a subscribable action that invokes when a busying action is finished.
    //attacker can subscribe to it so that the weapon switch happens as soon as weapon is unbusied?
}
