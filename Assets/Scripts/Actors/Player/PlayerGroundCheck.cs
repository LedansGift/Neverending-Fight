using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private bool checkerActive = false;
    private bool groundBelow = true;
    private int floatingTickDamage = 1;
    private float checkTimer = 0f;
    private const float CHECK_WAIT_DURATION = 0.1f;
    private const float GROUND_DETECT_RADIUS = 0.5f;
    private Transform safeGroundTransform;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private ParticleSystem floatingVFX;

    private void Awake()
    {
        safeGroundTransform = new GameObject("LastPlayerSafePosition").transform;
        floatingVFX.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (checkerActive)
        {
            checkTimer += Time.deltaTime;

            if (checkTimer > CHECK_WAIT_DURATION)
            {
                checkTimer = 0f;
                PerformGroundCheck();
            }
        }
    }

    private bool CheckIfGroundBelow()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(
            transform.position,
            GROUND_DETECT_RADIUS,
            groundLayerMask
        );

        return detectedColliders.Length > 0;
    }

    private void PerformGroundCheck()
    {
        bool groundBelow = CheckIfGroundBelow();

        if (GetIsGroundBelow() != groundBelow)
        {
            this.groundBelow = groundBelow;

            ToggleGroundBelowState(groundBelow);
        }

        if (groundBelow)
        {
            safeGroundTransform.position = transform.position;
        }
        else
        {
            //Deal damage
            playerHealth.TakeTickDamage(floatingTickDamage);
        }
    }

    private void ToggleGroundBelowState(bool toggle)
    {
        floatingVFX.gameObject.SetActive(!toggle);

        if (!toggle)
        {
            floatingVFX.Play();
        }
    }

    public bool GetIsGroundBelow()
    {
        return groundBelow;
    }

    public Transform GetSafeGroundTransform()
    {
        return safeGroundTransform;
    }

    public void SetCheckActive(bool isActive)
    {
        checkerActive = isActive;
    }
}
