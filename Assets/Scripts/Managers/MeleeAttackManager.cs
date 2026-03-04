using System;
using System.Collections;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    public static MeleeAttackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += StopAllAttacks;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= StopAllAttacks;
    }

    private IEnumerator TelegraphAttacks(
        MeleeAttack[] attacks,
        Transform attackOrigin,
        int attackDamage,
        Action onAttackComplete
    )
    {
        yield return null;

        Action lastAttackAction = null;

        for (int i = 0; i < attacks.Length; i++)
        {
            MeleeAttack attack = attacks[i];

            Vector3 zoneSpawnLocation;
            Vector3 zoneSpawnRotation;
            if (attacks[i].relativePosition)
            {
                zoneSpawnLocation = attackOrigin.position + attack.attackPosition;
                zoneSpawnRotation =
                    Quaternion.Euler(0f, attack.attackYRotation, 0f) * attackOrigin.forward;
            }
            else
            {
                zoneSpawnLocation = attack.attackPosition;
                zoneSpawnRotation =
                    Quaternion.Euler(0f, attack.attackYRotation, 0f) * Vector3.forward;
            }

            DamageZoneManager.Instance.SpawnDamageZone(
                zoneSpawnLocation,
                zoneSpawnRotation,
                attack.damageZoneType,
                attack.damageZoneArea,
                attack.zoneWarningTime
            );

            if (i >= (attacks.Length - 1))
            {
                lastAttackAction = onAttackComplete;
            }

            StartCoroutine(PerformAttack(attack, attackOrigin, attackDamage, lastAttackAction));

            yield return new WaitForSeconds(attack.delayToNextAttack);
        }
    }

    private IEnumerator PerformAttack(
        MeleeAttack attack,
        Transform attackOrigin,
        int attackDamage,
        Action onAttackComplete
    )
    {
        yield return new WaitForSeconds(attack.zoneWarningTime);

        Debug.Log("Attack hit");

        if (onAttackComplete != null)
        {
            onAttackComplete();
        }
    }

    public void StartAttackPattern(
        MeleeAttack[] attacks,
        Transform attackOrigin,
        int attackDamage,
        Action onAttackComplete
    )
    {
        StartCoroutine(TelegraphAttacks(attacks, attackOrigin, attackDamage, onAttackComplete));
    }

    private void StopAllAttacks()
    {
        StopAllCoroutines();
    }
}
