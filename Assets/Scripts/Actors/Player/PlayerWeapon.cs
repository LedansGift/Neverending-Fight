using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    protected bool isBusy = false;
    protected bool canSwap = true;
    protected LayerMask attackLayerMask;

    [SerializeField]
    protected PlayerStats playerStats;

    [SerializeField]
    protected Animator weaponAnimator;

    public abstract void WeaponAttackStart();
    public abstract void WeaponAttackEnd();

    public abstract void WeaponSpecial();

    public void SetAttackLayerMask(int layermask)
    {
        attackLayerMask = layermask;
    }

    public virtual void ActivateWeapon()
    {
        weaponAnimator.SetTrigger("draw");
        isBusy = false;
        canSwap = true;
        //set bools
    }

    public virtual void StowWeapon()
    {
        weaponAnimator.SetTrigger("stow");
        isBusy = true;
        canSwap = false;

        //set bools
    }

    public bool CanSwap()
    {
        return canSwap;
    }

    //have a subscribable action that invokes when a busying action is finished.
    //attacker can subscribe to it so that the weapon switch happens as soon as weapon is unbusied?
}
