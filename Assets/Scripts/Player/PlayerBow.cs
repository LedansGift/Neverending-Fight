public class PlayerBow : PlayerWeapon
{
    public override void WeaponAttackStart()
    {
        weaponAnimator.SetTrigger("attack");
    }

    public override void WeaponAttackEnd()
    {
        weaponAnimator.SetTrigger("loose");
    }

    public override void WeaponSpecial() { }
}
