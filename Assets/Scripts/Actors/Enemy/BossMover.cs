using System;
using System.Collections;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    private bool moveBoss = false;
    private float moveProgress = 0f;
    private float moveSpeed;
    private float endMoveDelay;
    private Vector3 startPosition;
    private Vector3 movementTarget;
    private Quaternion startRotation;
    private Quaternion rotationTarget;
    private Action onMovementFinished;

    [SerializeField]
    private Rigidbody bossRB;

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
    }

    private IEnumerator StartMovement(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        if (moveSpeed > 0f)
        {
            moveProgress = 0f;
            startPosition = transform.position;
            startRotation = transform.rotation;
            moveBoss = true;
        }
        else
        {
            bossRB.position = movementTarget;
            bossRB.rotation = rotationTarget;
            StartCoroutine(FinishMovement());
        }
    }

    private IEnumerator FinishMovement()
    {
        yield return new WaitForSeconds(endMoveDelay);

        if (onMovementFinished != null)
        {
            onMovementFinished();
        }
    }
}
