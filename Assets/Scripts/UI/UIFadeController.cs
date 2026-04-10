using UnityEngine;

public class UIFadeController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader uiFader;

    private void OnEnable()
    {
        BossFormManager.OnNewPhaseStart += FadeInUI;
        BossFormManager.OnPhaseFinished += FadeAwayUI;
    }

    private void OnDisable()
    {
        BossFormManager.OnNewPhaseStart -= FadeInUI;
        BossFormManager.OnPhaseFinished -= FadeAwayUI;
    }

    private void FadeInUI()
    {
        uiFader.ToggleFade(true);
    }

    private void FadeAwayUI()
    {
        uiFader.ToggleFade(false);
    }
}
