using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashUI : MonoBehaviour
{
    private bool dashCooldown = false;
    private float cooldownTimer = 0f;
    private float cooldownDuration = 0f;
    private Transform playerTransform;

    [SerializeField]
    private Slider dashSlider;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
        dashSlider.value = 1f;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnEnable()
    {
        PlayerMovement.OnDashCooldownStart += ActivateDashCooldown;
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        PlayerMovement.OnDashCooldownStart -= ActivateDashCooldown;
    }

    private void Update()
    {
        if (dashCooldown)
        {
            UpdateDashUI();
        }
    }

    private void UpdateDashUI()
    {
        cooldownTimer += Time.deltaTime;
        float valueLerp = cooldownTimer / cooldownDuration;

        if (valueLerp >= 1f)
        {
            valueLerp = 1f;
            dashCooldown = false;
            fader.ToggleFade(false);
        }

        dashSlider.value = valueLerp;
    }

    private void SetUIPosition()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(playerTransform.position);

        transform.position = screenPosition;
    }

    private void ActivateDashCooldown(object sender, float dashCD)
    {
        if (!playerTransform)
        {
            return;
        }

        cooldownDuration = dashCD;
        cooldownTimer = 0f;
        dashCooldown = true;

        fader.ToggleFade(true);
    }

    private void OnCameraUpdated(CinemachineBrain arg0)
    {
        SetUIPosition();
    }
}
