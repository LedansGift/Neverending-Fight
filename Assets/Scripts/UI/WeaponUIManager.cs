using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    private bool tomeCharging = false;
    private int activeWeaponUI = -1;

    private float tomeChargeTimer = 0f;
    private float tomeChargeCooldown = 0f;

    [SerializeField]
    private Color inactiveUIColour;

    [SerializeField]
    private AnimationCurve scaleChangeCurve;

    [SerializeField]
    private WeaponUI[] weaponUIs;

    private void Awake()
    {
        foreach (WeaponUI weaponUI in weaponUIs)
        {
            weaponUI.SetInactiveUIVars(inactiveUIColour, scaleChangeCurve);
            weaponUI.SetUIActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerWeapon.OnWeaponAbilityCharge += UpdateAbilityCharge;
        PlayerAttacker.OnSwitchWeapon += UpdateWeaponUI;
    }

    private void OnDisable()
    {
        PlayerWeapon.OnWeaponAbilityCharge -= UpdateAbilityCharge;
        PlayerAttacker.OnSwitchWeapon -= UpdateWeaponUI;
    }

    private void Update()
    {
        if (tomeCharging)
        {
            UpdateTomeAbility();
        }
    }

    private void UpdateTomeAbility()
    {
        tomeChargeTimer += Time.deltaTime;
        float chargeLerp = tomeChargeTimer / tomeChargeCooldown;

        if (chargeLerp >= 1f)
        {
            tomeCharging = false;
            chargeLerp = 1f;
        }

        weaponUIs[2].SetAbilityCharge(chargeLerp);
    }

    private void UpdateWeaponUI(object sender, int weaponType)
    {
        if (activeWeaponUI >= 0)
        {
            weaponUIs[activeWeaponUI].SetUIActive(false);
            activeWeaponUI = -1;
        }

        if ((weaponType < 0) || (weaponType > 2))
        {
            return;
        }

        weaponUIs[weaponType].SetUIActive(true);

        activeWeaponUI = weaponType;
    }

    private void UpdateAbilityCharge(object sender, WeaponAbilityCharge abilityCharge)
    {
        int weaponType = abilityCharge.weaponIndex;

        if ((weaponType < 0) || (weaponType > 2))
        {
            return;
        }

        if (weaponType == 2)
        {
            tomeChargeCooldown = abilityCharge.weaponCharge;
            tomeChargeTimer = 0f;
            tomeCharging = true;
        }
        else
        {
            weaponUIs[weaponType].SetAbilityCharge(abilityCharge.weaponCharge);
        }
    }
}
