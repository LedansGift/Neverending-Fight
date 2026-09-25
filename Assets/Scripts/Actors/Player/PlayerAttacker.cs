using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private bool canAttack = false;

    private int weaponLockStatus = 2;

    private PlayerWeapon activeWeapon = null;

    [SerializeField]
    private LayerMask attackLayerMask;

    [SerializeField]
    private PlayerWeapon[] playerWeapons;

    public static EventHandler<int> OnSwitchWeapon;

    private void Awake()
    {
        foreach (PlayerWeapon weapon in playerWeapons)
        {
            weapon.SetAttackLayerMask(attackLayerMask);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedEnable());
    }

    private IEnumerator DelayedEnable()
    {
        yield return null;
        InputManager.Instance.OnAttackEvent += WeaponAttack;
        InputManager.Instance.OnAttackReleaseEvent += WeaponAttackRelease;
        InputManager.Instance.OnSpecialEvent += WeaponSpecial;

        InputManager.Instance.OnSwapWeaponEvent += SwapWeapon;
        InputManager.Instance.OnSelectWeaponEvent += SelectWeapon;

        TutorialFightManager.OnToggleWeaponLock += UpdateWeaponLock;

        ToggleCanAttack(false);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= WeaponAttack;
        InputManager.Instance.OnAttackReleaseEvent -= WeaponAttackRelease;
        InputManager.Instance.OnSpecialEvent -= WeaponSpecial;

        InputManager.Instance.OnSwapWeaponEvent -= SwapWeapon;
        InputManager.Instance.OnSelectWeaponEvent -= SelectWeapon;

        TutorialFightManager.OnToggleWeaponLock -= UpdateWeaponLock;
    }

    private void WeaponAttack()
    {
        if (!canAttack || !activeWeapon)
        {
            return;
        }

        activeWeapon.WeaponAttackStart();
    }

    private void WeaponAttackRelease()
    {
        if (!canAttack || !activeWeapon)
        {
            return;
        }

        activeWeapon.WeaponAttackEnd();
    }

    private void WeaponSpecial()
    {
        if (!canAttack || !activeWeapon)
        {
            return;
        }

        activeWeapon.WeaponSpecial();
    }

    private void ChangeWeapon(PlayerWeapon newWeapon)
    {
        //Add buffer for switching if a weapon is mid-attack?

        if (activeWeapon != null)
        {
            activeWeapon.StowWeapon();
        }

        activeWeapon = newWeapon;
        activeWeapon.ActivateWeapon();

        OnSwitchWeapon?.Invoke(this, activeWeapon.GetWeaponIndex());
    }

    private void SelectWeapon(object sender, int newWeapon)
    {
        if (!canAttack)
        {
            return;
        }

        if (!activeWeapon.CanSwap())
        {
            return;
        }

        int newWeaponIndex = newWeapon - 1;

        if (newWeaponIndex > weaponLockStatus)
        {
            return;
        }

        PlayerWeapon weapon = playerWeapons[newWeaponIndex];

        if (weapon != activeWeapon)
        {
            ChangeWeapon(weapon);
        }
    }

    private void SwapWeapon(object sender, float newWeapon)
    {
        if (!canAttack)
        {
            return;
        }

        if (!activeWeapon)
        {
            ChangeWeapon(playerWeapons[0]);
            return;
        }

        if (!activeWeapon.CanSwap())
        {
            return;
        }

        int newWeaponIndex = (int)
            AdditionalMath.Modulus(
                Array.IndexOf(playerWeapons, activeWeapon) + (int)newWeapon,
                weaponLockStatus + 1
            );

        PlayerWeapon weapon = playerWeapons[newWeaponIndex];

        if (weapon != activeWeapon)
        {
            ChangeWeapon(weapon);
        }
    }

    public void ResetWeapons()
    {
        foreach (PlayerWeapon weapon in playerWeapons)
        {
            weapon.ResetWeapon();
        }

        activeWeapon = null;
        OnSwitchWeapon?.Invoke(this, -1);
    }

    public void SetMouseTarget(Transform targetTransform)
    {
        foreach (PlayerWeapon weapon in playerWeapons)
        {
            weapon.SetMouseTarget(targetTransform);
        }
    }

    public void ToggleCanAttack(bool toggle)
    {
        canAttack = toggle;

        if (toggle)
        {
            ChangeWeapon(playerWeapons[0]);
        }
    }

    private void UpdateWeaponLock(object sender, int lockInt)
    {
        weaponLockStatus = lockInt;
    }
}
