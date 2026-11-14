using System;
using UnityEngine;

public class TextCutsceneUI : MonoBehaviour
{
    private Action OnCutsceneFinished;

    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private TextCutsceneModule[] cutsceneModules;

    private static EventHandler<TextCutsceneArgs> OnStartTextCutscene;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        OnStartTextCutscene += StartTextCutscene;
        TextCutsceneModule.OnModuleFinished += EndTextCutscene;
    }

    private void OnDisable()
    {
        OnStartTextCutscene -= StartTextCutscene;
        TextCutsceneModule.OnModuleFinished -= EndTextCutscene;
    }

    private void StartTextCutscene(object sender, TextCutsceneArgs cutsceneArgs)
    {
        foreach (TextCutsceneModule module in cutsceneModules)
        {
            module.ResetModule();
        }

        OnCutsceneFinished = cutsceneArgs.onCutsceneFinished;
        fader.ToggleFade(true);

        if (cutsceneArgs.cutsceneIndex >= cutsceneModules.Length)
        {
            Debug.Log("Index Out of Range");
            return;
        }

        cutsceneModules[cutsceneArgs.cutsceneIndex].StartTextCutscene();
    }

    private void EndTextCutscene()
    {
        fader.ToggleFade(false);

        if (OnCutsceneFinished != null)
        {
            OnCutsceneFinished();
            OnCutsceneFinished = null;
        }
    }

    public static void StartTextCutsceneEvent(int cutsceneIndex, Action onCutsceneFinished)
    {
        OnStartTextCutscene?.Invoke(null, new TextCutsceneArgs(cutsceneIndex, onCutsceneFinished));
    }
}
