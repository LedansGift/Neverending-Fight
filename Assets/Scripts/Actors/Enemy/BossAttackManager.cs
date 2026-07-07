using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    private const float FAIL_DAMAGE_MULT_INCREASE = 0.5f;
    private List<AttackFailTracker> savedAttacks = new List<AttackFailTracker>();
    private List<AttackFailTracker> currentAttacks = new List<AttackFailTracker>();

    private PlayerHealth playerHealth;

    [SerializeField]
    private BossMeleeAttacker meleeAttacker;

    [SerializeField]
    private StateDictionary stateDictionary;

    [SerializeField]
    private BossMover mover;

    private void Start()
    {
        if (PlayerIdentifier.PlayerTransform != null)
        {
            playerHealth = PlayerIdentifier.PlayerTransform.GetComponent<PlayerHealth>();
        }
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += ResetAttacker;
        BossAttackNode.OnAttackFailCheck += CheckAttackFailure;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= ResetAttacker;
        BossAttackNode.OnAttackFailCheck -= CheckAttackFailure;
    }

    private List<AttackFailTracker> GetFullAttackList()
    {
        List<AttackFailTracker> fullList = new List<AttackFailTracker>();
        fullList.AddRange(savedAttacks);
        fullList.AddRange(currentAttacks);

        return fullList;
    }

    private void RegisterAttackFailure(BossAttackNode attackNode)
    {
        List<AttackFailTracker> attacks = currentAttacks;
        int attackIndex = currentAttacks.FindIndex(p => p.GetAttackNode() == attackNode);
        if (attackIndex < 0)
        {
            attacks = savedAttacks;
            attackIndex = savedAttacks.FindIndex(p => p.GetAttackNode() == attackNode);
            if (attackIndex < 0)
            {
                return;
            }
        }

        Debug.Log("Attack Fail Incremented for index " + attackIndex);

        AttackFailTracker attackTracker = attacks[attackIndex];

        attackTracker.IncrementAttackFailure();
        attackTracker.SetPreviousAttackFailStatus(true);

        //attacks[attackIndex] = attackFail;
    }

    private BossAttackNode ResolveAttackNode(BossAttackNode inAttackNode, out int damageScale)
    {
        List<AttackFailTracker> attacks = GetFullAttackList();
        int attackIndex = attacks.FindIndex(p => p.GetAttackNode() == inAttackNode);

        BossAttackNode outAttackNode = inAttackNode;
        damageScale = 0;

        if (attackIndex < 0)
        {
            currentAttacks.Add(new AttackFailTracker(inAttackNode, 0));
            //Debug.Log("New Node Created");
            return outAttackNode;
        }

        AttackFailTracker attack = attacks[attackIndex];

        bool previousAttackFail = attack.GetPreviousAttackFailed();
        attack.SetPreviousAttackFailStatus(false);

        if (
            !previousAttackFail
            && attack.GetAttackNode().TryGetEmpoweredAttack(out BossAttackNode empoweredNode)
        )
        {
            return ResolveAttackNode(empoweredNode, out damageScale);
        }

        damageScale = attack.GetAttacksFailed();
        // Debug.Log("Damage Scale: " + damageScale);
        //Debug.Log("Attacks Failed: " + attack.GetAttacksFailed() + " for index " + attackIndex);

        return outAttackNode;
    }

    public void PerformAttackNode(BossAttackNode attackNode, Action onAttackFinished)
    {
        BossAttackNode resolvedNode = ResolveAttackNode(attackNode, out int damageScale);
        float damageMult = 1f + (FAIL_DAMAGE_MULT_INCREASE * damageScale);

        resolvedNode.PerformAttack(this, onAttackFinished, damageMult);
    }

    public BossMeleeAttacker GetBossMeleeAttacker()
    {
        return meleeAttacker;
    }

    public BossMover GetBossMover()
    {
        return mover;
    }

    public StateDictionary GetStateDictionary()
    {
        return stateDictionary;
    }

    public void StartBossIdle(float idleTime, Action onIdleFinished)
    {
        StartCoroutine(IdleBoss(idleTime, onIdleFinished));
    }

    public void PhaseEndCleanup()
    {
        savedAttacks.AddRange(currentAttacks);
        ResetAttacker();
        meleeAttacker.ResetMeleeAttacker();
    }

    private IEnumerator IdleBoss(float idleTime, Action onIdleFinished)
    {
        yield return new WaitForSeconds(idleTime);
        onIdleFinished();
    }

    private void CheckAttackFailure(object sender, EventArgs eventArgs)
    {
        if (!playerHealth)
        {
            Debug.Log("No Player health Set");
            return;
        }

        if (playerHealth.GetAttackFailStatus())
        {
            Debug.Log("Attack failed: " + sender);
            RegisterAttackFailure(sender as BossAttackNode);
            playerHealth.ResetAttackFailStatus();
        }
    }

    private void ResetAttacker()
    {
        StopAllCoroutines();
        currentAttacks = new List<AttackFailTracker>();
    }
}
