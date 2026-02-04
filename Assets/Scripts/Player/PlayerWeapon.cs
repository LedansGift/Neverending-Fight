using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    public abstract void WeaponAttackStart();
    public abstract void WeaponAttackEnd();

    public abstract void WeaponSpecial();

    public virtual void ActivateWeapon()
    {
        //play animation
        //set bools
    }

    public virtual void StowWeapon()
    {
        //play animation
        //set bools
    }
}
