using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    private int activeWeaponUI = -1;

    [SerializeField]
    private Transform[] weaponUIHolders;

    [SerializeField]
    private Image[] weaponAbilityImages;

    private void OnEnable()
    {
        PlayerWeapon.OnWeaponAbilityCharge += UpdateAbilityCharge;
        //on weapon switch
    }

    private void OnDisable()
    {
        PlayerWeapon.OnWeaponAbilityCharge -= UpdateAbilityCharge;
    }

    private void UpdateAbilityCharge(object sender, WeaponAbilityCharge abilityCharge)
    {
        //update ability charges
    }
}
