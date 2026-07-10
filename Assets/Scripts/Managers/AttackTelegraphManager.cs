using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTelegraphManager : MonoBehaviour
{
    public static AttackTelegraphManager Instance { get; private set; }

    private Dictionary<MeleeAttack, DamageZone> activeTelegraphs =
        new Dictionary<MeleeAttack, DamageZone>();

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
        RestartManager.OnResetPhase += StopAllTelegraphs;
        BossFormManager.OnPhaseFinished += StopAllTelegraphs;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= StopAllTelegraphs;
        BossFormManager.OnPhaseFinished -= StopAllTelegraphs;
    }

    private IEnumerator TelegraphAttacks(Transform attacker, MeleeAttack[] attacks)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            MeleeAttack attack = attacks[i];

            StartAttack(attacker, attack);

            yield return new WaitForSeconds(attack.delayToNextAttack);
        }
    }

    public void StartAttack(Transform attacker, MeleeAttack attack)
    {
        Vector3 zoneSpawnLocation;
        Vector3 zoneSpawnRotation;
        if (attack.relativePosition)
        {
            zoneSpawnLocation = attacker.transform.position + attack.attackPosition;
            zoneSpawnRotation =
                Quaternion.Euler(0f, attack.attackYRotation, 0f) * attacker.transform.forward;
        }
        else
        {
            zoneSpawnLocation = attack.attackPosition;
            zoneSpawnRotation = Quaternion.Euler(0f, attack.attackYRotation, 0f) * Vector3.forward;
        }

        DamageZone damageZone = DamageZoneManager.Instance.SpawnDamageZone(
            zoneSpawnLocation,
            zoneSpawnRotation,
            attack.damageZoneType,
            attack.damageZoneArea,
            attack.zoneWarningTime
        );

        activeTelegraphs.Add(attack, damageZone);
    }

    public void EndAttackEarly(MeleeAttack attack)
    {
        if (
            activeTelegraphs.TryGetValue(attack, out DamageZone damageZone)
            && damageZone.GetIsZoneActive()
        )
        {
            damageZone.DeactivateZone();
        }
    }

    public void StartAttackPattern(Transform attackerTransform, MeleeAttack[] attacks)
    {
        // var activeKeys = activeTelegraphs.Keys;
        // foreach (MeleeAttack attack in activeKeys)
        // {
        //     if (!activeTelegraphs[attack].GetIsZoneActive())
        //     {
        //         activeTelegraphs.Remove(attack);
        //     }
        // }

        StartCoroutine(TelegraphAttacks(attackerTransform, attacks));
    }

    private void StopAllTelegraphs()
    {
        StopAllCoroutines();

        activeTelegraphs = new Dictionary<MeleeAttack, DamageZone>();
    }
}
