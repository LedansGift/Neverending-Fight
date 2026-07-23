using System;
using UnityEngine;

public class UIFadeController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader uiFader;

    private static EventHandler<bool> OnFadeUI;

    private void OnEnable()
    {
        // BossFormManager.OnNewPhaseStart += FadeInUI;
        // BossFormManager.OnPhaseFinished += FadeAwayUI;
        OnFadeUI += ToggleUI;
    }

    private void OnDisable()
    {
        // BossFormManager.OnNewPhaseStart -= FadeInUI;
        // BossFormManager.OnPhaseFinished -= FadeAwayUI;
        OnFadeUI -= ToggleUI;
    }

    private void ToggleUI(object sender, bool toggle)
    {
        uiFader.ToggleFade(toggle);
    }

    // private void FadeInUI()
    // {
    //     uiFader.ToggleFade(true);
    // }

    // private void FadeAwayUI()
    // {
    //     uiFader.ToggleFade(false);
    // }

    public static void ToggleUI(bool toggle)
    {
        OnFadeUI?.Invoke(null, toggle);
    }
}
