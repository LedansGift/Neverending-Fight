using System.Collections;
using UnityEngine;

public class AttackTelegraphManager : MonoBehaviour
{
    public static AttackTelegraphManager Instance { get; private set; }

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

            if (attack.damageZoneType != DamageZoneType.raidwide)
            {
                StartAttack(attacker, attack);
            }
            else
            {
                RaidwideDamageUI.SetDamageValue(attack.attackDamage);
                RaidwideDamageUI.StartDamageUI(attack.zoneWarningTime);
            }

            yield return new WaitForSeconds(attack.delayToNextAttack);
        }
    }

    public DamageZone StartAttack(Transform attacker, MeleeAttack attack)
    {
        Vector3 zoneSpawnLocation;
        Vector3 zoneSpawnRotation;

        if (attack.relativePosition)
        {
            if (attack.relativePositionToForward)
            {
                Quaternion forwardRotation = Quaternion.FromToRotation(
                    Vector3.forward,
                    attacker.transform.forward
                );
                Vector3 newAttackPosition = forwardRotation * attack.attackPosition;

                zoneSpawnLocation = attacker.transform.position + newAttackPosition;
            }
            else
            {
                zoneSpawnLocation = attacker.transform.position + attack.attackPosition;
            }

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

        return damageZone;
    }

    public void StartAttackPattern(Transform attackerTransform, MeleeAttack[] attacks)
    {
        StartCoroutine(TelegraphAttacks(attackerTransform, attacks));
    }

    private void StopAllTelegraphs()
    {
        StopAllCoroutines();
    }
}
