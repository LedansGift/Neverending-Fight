using System;
using System.Collections;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    private bool moveBoss = false;
    private bool lockOnActive = false;
    private float moveProgress = 0f;
    private float moveSpeed;
    private float endMoveDelay;
    private Vector3 startPosition;
    private Vector3 movementTarget;
    private Quaternion startRotation;
    private Quaternion rotationTarget;

    private Transform lockOnTarget;

    private Action onMovementFinished;

    [SerializeField]
    private Rigidbody bossRB;

    [SerializeField]
    private Transform bossSpawnTransform;

    [SerializeField]
    private Animator bossAnimator;

    [SerializeField]
    private DashTrail bossTeleportStartEffect;

    [SerializeField]
    private DashTrail bossTeleportEndEffect;

    [SerializeField]
    private TransformShrinkScaler bossShrinker;

    private void Awake()
    {
        bossSpawnTransform.SetParent(null);
    }

    public void HandleMoveNode(
        Vector3 newPosition,
        Quaternion newRotation,
        float moveSpeed,
        Action onMovementFinished,
        float initialDelay = 0f,
        float endDelay = 0f
    )
    {
        moveBoss = false;
        movementTarget = newPosition;
        rotationTarget = newRotation;
        this.moveSpeed = moveSpeed;
        this.onMovementFinished = onMovementFinished;
        endMoveDelay = endDelay;

        StartCoroutine(StartMovement(initialDelay));
    }

    private void MoveBoss()
    {
        moveProgress += moveSpeed * Time.fixedDeltaTime * Time.timeScale;

        if (moveProgress >= 1f)
        {
            moveProgress = 1f;
            moveBoss = false;
            StartCoroutine(FinishMovement());
        }

        Vector3 positionLerp = Vector3.Lerp(startPosition, movementTarget, moveProgress);
        Quaternion rotationLerp = Quaternion.Slerp(startRotation, rotationTarget, moveProgress);

        bossRB.Move(positionLerp, rotationLerp);

        // bossRB.MovePosition();
        // bossRB.MoveRotation
    }

    private void FixedUpdate()
    {
        if (moveBoss)
        {
            MoveBoss();
        }
        else if (lockOnActive)
        {
            RotateTowardsTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        // Quaternion rotationLerp = Quaternion.Slerp(
        //     bossRB.rotation,
        //     ,
        //     0.5f
        // );
        bossRB.MoveRotation(
            Quaternion.LookRotation(
                (lockOnTarget.transform.position - bossRB.transform.position).normalized
            )
        );
    }

    private IEnumerator StartMovement(float initialDelay)
    {
        if (moveSpeed > 0f)
        {
            yield return new WaitForSeconds(initialDelay);

            moveProgress = 0f;
            startPosition = transform.position;
            startRotation = transform.rotation;
            moveBoss = true;
        }
        else
        {
            StartCoroutine(TeleportBoss(initialDelay));
        }
    }

    private IEnumerator TeleportBoss(float initialDelay)
    {
        bossTeleportStartEffect.ActivateDashTrail(Vector2.up);
        bossAnimator.SetTrigger("teleport");
        bossShrinker.ShrinkTransform();

        yield return new WaitForSeconds(initialDelay);

        bossShrinker.UnshrinkTransform();
        bossRB.position = movementTarget;
        bossRB.rotation = rotationTarget;

        yield return new WaitForFixedUpdate();

        bossTeleportEndEffect.ActivateDashTrail(Vector2.up);

        StartCoroutine(FinishMovement());
    }

    private IEnumerator FinishMovement()
    {
        yield return new WaitForSeconds(endMoveDelay);

        if (onMovementFinished != null)
        {
            onMovementFinished();
        }
    }

    public void LockOnTarget(Transform target)
    {
        lockOnTarget = target;
        lockOnActive = true;
    }

    public void CancelLockOn()
    {
        lockOnActive = false;
    }

    // public void ResetLookDirection()
    // {

    // }

    public Vector3 GetBossPosition()
    {
        return bossRB.position;
    }

    public Quaternion GetBossRotation()
    {
        return bossRB.rotation;
    }

    public void ResetMover()
    {
        StopAllCoroutines();
        moveBoss = false;
        lockOnActive = false;
        onMovementFinished = null;

        bossRB.position = bossSpawnTransform.position;
        bossRB.rotation = bossSpawnTransform.rotation;
    }
}
