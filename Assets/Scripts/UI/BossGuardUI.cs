using UnityEngine;
using UnityEngine.UI;

public class BossGuardUI : MonoBehaviour
{
    private bool guardActive = false;

    [SerializeField]
    private Slider guardSlider;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
        guardSlider.value = 1f;
    }

    private void OnEnable()
    {
        BossGuardState.OnToggleGuardState += ToggleGuardUI;
        BossGuardState.OnNewGuardValue += UpdateGuardValue;
    }

    private void OnDisable()
    {
        BossGuardState.OnToggleGuardState -= ToggleGuardUI;
        BossGuardState.OnNewGuardValue -= UpdateGuardValue;
    }

    // private void Update()
    // {
    //     if (guardActive)
    //     {
    //         UpdateDashUI();
    //     }
    // }

    // private void UpdateDashUI()
    // {
    //     cooldownTimer += Time.deltaTime;
    //     float valueLerp = cooldownTimer / cooldownDuration;

    //     if (valueLerp >= 1f)
    //     {
    //         valueLerp = 1f;
    //         dashCooldown = false;
    //         fader.ToggleFade(false);
    //     }

    //     dashSlider.value = valueLerp;
    // }

    private void UpdateGuardValue(object sender, float newValue)
    {
        if (!guardActive)
        {
            return;
        }

        guardSlider.value = newValue;
    }

    private void ToggleGuardUI(object sender, bool toggle)
    {
        if (toggle)
        {
            guardSlider.value = 0;
        }

        guardActive = toggle;
        fader.ToggleFade(toggle);
    }
}
