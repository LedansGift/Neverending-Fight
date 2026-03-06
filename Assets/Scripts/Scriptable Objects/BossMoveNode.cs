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

        Vector3 movement = ResolvePosition(attacker.transform);
        Quaternion rotation = ResolvedRotation(attacker.transform);

        BossMover mover = attacker.GetBossMover();
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
        FinishAttack(this, false);
    }

    private Vector3 ResolvePosition(Transform bossTransform)
    {
        if (!relativeMovement)
        {
            return newPosition;
        }

        Vector3 resolvedPosition = bossTransform.position + bossTransform.rotation * newPosition;
        return resolvedPosition;
    }

    private Quaternion ResolvedRotation(Transform bossTransform)
    {
        if (!relativeMovement)
        {
            return Quaternion.Euler(0f, newRotation, 0f);
        }

        return Quaternion.Euler(0f, bossTransform.eulerAngles.y + newRotation, 0f);
    }
}
