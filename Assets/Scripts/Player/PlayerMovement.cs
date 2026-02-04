using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove;
    private bool isDashing = false;
    private bool dashAvailable;

    private float dashModifier = 1f;
    private float dashTimer = 0f;
    private Vector2 movementDirection;
    private Vector2 movementDashBuffer;

    private PlayerStats stats;
    private Coroutine dashCoroutine;
    private InputManager inputManager;

    [SerializeField]
    private Rigidbody playerRB;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        ToggleCanMove(false);
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        InputManager.Instance.OnDashEvent += TryDash;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDashEvent -= TryDash;

        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }

    private void Update()
    {
        if (canMove)
        {
            if (!dashAvailable)
            {
                dashTimer += Time.deltaTime;

                float dashRechargeTime = stats.GetDashRechargeTime();

                //dashSlider.value = dashTimer / dashRechargeTime;

                if (dashTimer >= dashRechargeTime)
                {
                    dashAvailable = true;
                    dashTimer = 0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveRB();
        }
    }

    private Vector2 MoveRB()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        movementDirection = movementValue;

        if (isDashing)
        {
            movementValue = movementDashBuffer;
        }

        playerRB.MovePosition(
            playerRB.position
                + new Vector3(movementValue.x, 0f, movementValue.y)
                    * stats.GetMovementSpeed()
                    * dashModifier
                    * Time.fixedDeltaTime
        );

        if (movementValue.sqrMagnitude > 0f)
        {
            //playerAnimator.SetBool("moving", true);
        }

        return movementValue;
    }

    private void TryDash()
    {
        if (!dashAvailable || !canMove)
        {
            return;
        }

        if (movementDirection.sqrMagnitude < 0.01f)
        {
            return;
        }

        dashCoroutine = StartCoroutine(ApplyDash());

        dashAvailable = false;
    }

    private IEnumerator ApplyDash()
    {
        isDashing = true;
        movementDashBuffer = movementDirection;
        dashModifier = stats.GetDashSpeedModifier();

        yield return new WaitForSeconds(stats.GetDashTime());

        dashModifier = 1f;
        isDashing = false;
    }

    public void ToggleCanMove(bool enable)
    {
        canMove = enable;
    }
}
