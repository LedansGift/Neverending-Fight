using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFellWingState : BossState
{
    private int damage = 25;
    private int attackNumber = 1;
    private float damageWarningTime = 0.5f;
    private float telegraphDuration = 3f;
    private float timeBetweenSlashes = 1.5f;

    private MeleeAttack leftSlash;
    private MeleeAttack rightSlash;

    private List<int> wingStrikeDirection = new List<int>();

    public BossFellWingState(BossStateMachine stateMachine, int attackVersion = 0)
        : base(stateMachine)
    {
        leftSlash = new MeleeAttack(
            damage,
            DamageZoneType.box,
            new Vector2(50f, 50f),
            damageWarningTime,
            new Vector3(-50f, 0f, 0f),
            default,
            default,
            default,
            default,
            500f
        );

        rightSlash = leftSlash;
        rightSlash.attackPosition = new Vector3(50f, 0f, 0f);

        attackNumber = 1 + attackVersion;
    }

    public override void Enter()
    {
        BossCastBarUI.InitiateCastEvent(new CastInfo("Fell Wing", telegraphDuration));
        wingStrikeDirection = new List<int>();

        for (int i = 0; i < attackNumber; i++)
        {
            wingStrikeDirection.Add(Random.Range(0, 2));
        }

        //Perform wing telegraphs first

        bossStateMachine.StartCoroutine(PerformTelegraphs(attackNumber));
    }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }

    private IEnumerator PerformTelegraphs(int attackNumber)
    {
        float timeBetweenTelegraphs = telegraphDuration / (attackNumber + 1);

        yield return new WaitForSeconds(timeBetweenTelegraphs);

        for (int i = 0; i < attackNumber; i++)
        {
            Debug.Log("Telegraph: " + wingStrikeDirection[i]);
            yield return new WaitForSeconds(timeBetweenTelegraphs);
        }

        bossStateMachine.StartCoroutine(PerformAttacks());
    }

    private IEnumerator PerformAttacks()
    {
        for (int i = 0; i < wingStrikeDirection.Count; i++)
        {
            MeleeAttack activeAttack;

            if (wingStrikeDirection[i] <= 0)
            {
                activeAttack = leftSlash;
            }
            else
            {
                activeAttack = rightSlash;
            }

            MeleeAttack[] attackHolder = { activeAttack };

            bossStateMachine.GetMeleeAttacker().PerformMeleeAttacks(attackHolder, damageMult, null);

            yield return new WaitForSeconds(timeBetweenSlashes);
        }

        stateMachine.SwitchState(null);
    }
}
