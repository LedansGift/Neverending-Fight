using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private bool checkerActive = false;
    private bool groundBelow = true;
    private float checkTimer = 0f;
    private const float CHECK_WAIT_DURATION = 0.1f;
    private Transform safeGroundTransform;

    private void Awake()
    {
        safeGroundTransform = new GameObject("LastPlayerSafePosition").transform;
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

    private void PerformGroundCheck()
    {
        bool groundBelow = false;

        //Ground check

        this.groundBelow = groundBelow;

        if (groundBelow)
        {
            safeGroundTransform.position = transform.position;
        }
        else
        {
            //Deal damage
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
}
