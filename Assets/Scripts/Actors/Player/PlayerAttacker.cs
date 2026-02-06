using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private bool canAttack = false;
    private PlayerWeapon activeWeapon = null;

    [SerializeField]
    private LayerMask attackLayerMask;

    [SerializeField]
    private PlayerWeapon[] playerWeapons;

    private void Awake()
    {
        ToggleCanAttack(false);

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
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= WeaponAttack;
        InputManager.Instance.OnAttackReleaseEvent -= WeaponAttackRelease;
        InputManager.Instance.OnSpecialEvent -= WeaponSpecial;

        InputManager.Instance.OnSwapWeaponEvent -= SwapWeapon;
        InputManager.Instance.OnSelectWeaponEvent -= SelectWeapon;
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
    }

    private void SelectWeapon(object sender, int newWeapon)
    {
        if (!canAttack)
        {
            return;
        }

        PlayerWeapon weapon = playerWeapons[newWeapon - 1];

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

        int newWeaponIndex = (int)
            AdditionalMath.Modulus(
                Array.IndexOf(playerWeapons, activeWeapon) + (int)newWeapon,
                playerWeapons.Length
            );

        ChangeWeapon(playerWeapons[newWeaponIndex]);
    }

    public void ToggleCanAttack(bool toggle)
    {
        canAttack = toggle;
        //Set glaive as weapon?
    }
}
