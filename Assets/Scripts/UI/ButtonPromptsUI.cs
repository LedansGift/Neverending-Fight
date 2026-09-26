using UnityEngine;

public class ButtonPromptsUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader attackUI;

    [SerializeField]
    private CanvasGroupFader dashUI;

    [SerializeField]
    private CanvasGroupFader specialUI;

    [SerializeField]
    private CanvasGroupFader swapUI;

    private void Awake()
    {
        attackUI.SetCanvasGroupAlpha(0f);
        dashUI.SetCanvasGroupAlpha(0f);
        specialUI.SetCanvasGroupAlpha(0f);
        swapUI.SetCanvasGroupAlpha(0f);
    }

    void OnEnable()
    {
        TutorialFightManager.OnToggleButtonPrompts += ToggleButtonPrompts;
    }

    void OnDisable()
    {
        TutorialFightManager.OnToggleButtonPrompts -= ToggleButtonPrompts;
    }

    private void ToggleButtonPrompts(object sender, int promptEnable)
    {
        switch (promptEnable)
        {
            case 0:
                attackUI.ToggleFade(true);
                dashUI.ToggleFade(true);
                break;
            case 1:
                specialUI.ToggleFade(true);
                break;
            case 2:
                swapUI.ToggleFade(true);
                break;
            default:
                break;
        }
    }
}
