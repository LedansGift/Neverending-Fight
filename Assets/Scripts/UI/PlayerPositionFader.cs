using UnityEngine;

public class PlayerPositionFader : MonoBehaviour
{
    private bool faderActive = false;
    private bool healthFadeActive = false;
    private bool weaponFadeActive = false;
    private float resolveTimer = 0f;
    private float resolveCD = 0.25f;
    private float screenFadeThresholdXWeapon = 0.7f;
    private float screenFadeThresholdXHealth = 0.275f;
    private float screenFadeThresholdY = 0.25f;
    private float fadeStrength = 0.33f;
    private Transform playerTransform;

    [SerializeField]
    private CanvasGroupFader healthFader;

    [SerializeField]
    private CanvasGroupFader weaponFader;

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        TogglePositionFader(true);
    }

    private void Update()
    {
        if (faderActive)
        {
            resolveTimer += Time.deltaTime;

            if (resolveTimer > resolveCD)
            {
                ResolvePlayerPosition();
                resolveTimer = 0f;
            }
        }
    }

    private void ResolvePlayerPosition()
    {
        Vector2 playerCameraLocation = Camera.main.WorldToViewportPoint(playerTransform.position);

        if (playerCameraLocation.y < screenFadeThresholdY)
        {
            if (playerCameraLocation.x < screenFadeThresholdXHealth)
            {
                if (!healthFadeActive)
                {
                    healthFader.ToggleFade(true, fadeStrength);
                    healthFadeActive = true;
                }
            }
            else if (playerCameraLocation.x < screenFadeThresholdXWeapon)
            {
                if (!weaponFadeActive)
                {
                    weaponFader.ToggleFade(true, fadeStrength);
                    weaponFadeActive = true;
                }
            }
        }
        else
        {
            if (healthFadeActive)
            {
                healthFader.ToggleFade(true, 1f);
                healthFadeActive = false;
            }

            if (weaponFadeActive)
            {
                weaponFader.ToggleFade(true, 1f);
                weaponFadeActive = false;
            }
        }
    }

    public void TogglePositionFader(bool toggle)
    {
        faderActive = toggle;
    }
}
