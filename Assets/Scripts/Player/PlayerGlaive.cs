public class PlayerGlaive : PlayerWeapon
{
    public override void WeaponAttackStart()
    {
        weaponAnimator.SetTrigger("attack");
    }

    public override void WeaponAttackEnd() { }

    public override void WeaponSpecial() { }
}
