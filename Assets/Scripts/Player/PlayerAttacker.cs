using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private bool canAttack = false;
    private PlayerWeapon activeWeapon = null;

    [SerializeField]
    private PlayerWeapon[] playerWeapons;

    private void Awake()
    {
        ToggleCanAttack(false);
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

    private void WeaponAttack() { }

    private void WeaponAttackRelease() { }

    private void WeaponSpecial() { }

    private void SelectWeapon(object sender, int newWeapon) { }

    private void SwapWeapon(object sender, float newWeapon) { }

    public void ToggleCanAttack(bool toggle)
    {
        canAttack = toggle;
        //Set glaive as weapon?
    }
}
