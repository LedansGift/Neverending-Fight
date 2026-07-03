using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Move", menuName = "Boss Attack/Move", order = 0)]
public class BossMoveNode : BossAttackNode
{
    [SerializeField]
    private Vector3 newPosition;

    [SerializeField]
    private float newRotation;

    [SerializeField]
    private float moveSpeed = 0f;

    [SerializeField]
    private bool relativeMovement;

    [SerializeField]
    private float movementStartDelay = 0.5f;

    [SerializeField]
    private float movementEndDelay = 0.5f;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;

        BossMover mover = attacker.GetBossMover();

        Vector3 movement = ResolvePosition(mover.GetBossPosition(), mover.GetBossRotation());
        Quaternion rotation = ResolvedRotation(mover.GetBossRotation());

        mover.HandleMoveNode(
            movement,
            rotation,
            moveSpeed,
            FinishMovement,
            movementStartDelay,
            movementEndDelay
        );
    }

    private void FinishMovement()
    {
        FinishAttack();
    }

    private Vector3 ResolvePosition(Vector3 bossPosition, Quaternion bossRotation)
    {
        if (!relativeMovement)
        {
            return newPosition;
        }

        Vector3 resolvedPosition = bossPosition + bossRotation * newPosition;

        return resolvedPosition;
    }

    private Quaternion ResolvedRotation(Quaternion bossRotation)
    {
        if (!relativeMovement)
        {
            return Quaternion.Euler(0f, newRotation, 0f);
        }

        return Quaternion.Euler(0f, bossRotation.eulerAngles.y + newRotation, 0f);
    }
}
