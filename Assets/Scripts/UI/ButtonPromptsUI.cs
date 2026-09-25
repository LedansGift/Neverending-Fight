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
}
