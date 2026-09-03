using System;
using UnityEngine;

public class ResetPhaseUI : MonoBehaviour
{
    private int resetTracker = 1;

    [SerializeField]
    private Animator resetUIAnimator;

    [SerializeField]
    private CanvasGroupFader resetUIFader;

    private void Awake()
    {
        resetUIFader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        RestartManager.OnToggleRewindUI += ToggleRewindUI;
        PlayerTimepiece.OnResetRetries += ResetRetryTracker;
    }

    private void OnDisable()
    {
        RestartManager.OnToggleRewindUI -= ToggleRewindUI;
        PlayerTimepiece.OnResetRetries -= ResetRetryTracker;
    }

    private void ResetRetryTracker()
    {
        resetTracker = 1;
    }

    private void ToggleRewindUI(object sender, bool toggle)
    {
        resetUIFader.ToggleFade(toggle);

        if (toggle)
        {
            if (resetTracker >= 1)
            {
                resetUIAnimator.SetTrigger("2flame");
            }
            else
            {
                resetUIAnimator.SetTrigger("1flame");
            }

            resetTracker--;
        }
    }
}
