using System;
using UnityEngine;

public class TextCutsceneModule : MonoBehaviour
{
    private bool inputEventSub = false;

    private int cutsceneProgress = 0;

    [SerializeField]
    private CanvasGroupFader[] textCutsceneParagraphs;

    public static Action OnModuleFinished;

    private void EndTextCutscene()
    {
        InputManager.Instance.OnAttackEvent -= AdvanceCutscene;
        inputEventSub = false;

        OnModuleFinished?.Invoke();
    }

    private void AdvanceCutscene()
    {
        if (cutsceneProgress >= textCutsceneParagraphs.Length)
        {
            EndTextCutscene();
            return;
        }

        textCutsceneParagraphs[cutsceneProgress].ToggleFade(true);
        cutsceneProgress++;
    }

    public void StartTextCutscene()
    {
        if (!inputEventSub)
        {
            InputManager.Instance.OnAttackEvent += AdvanceCutscene;
            inputEventSub = true;
        }

        AdvanceCutscene();
    }

    public void ResetModule()
    {
        cutsceneProgress = 0;

        foreach (CanvasGroupFader fader in textCutsceneParagraphs)
        {
            fader.SetCanvasGroupAlpha(0f);
        }
    }
}
