using System;
using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader loadingFader;

    private static EventHandler<bool> OnToggleLoadingScreen;

    private void OnEnable()
    {
        OnToggleLoadingScreen += ToggleFade;
    }

    private void OnDisable()
    {
        OnToggleLoadingScreen -= ToggleFade;
    }

    private void ToggleFade(object sender, bool toggle)
    {
        loadingFader.ToggleFade(toggle);
    }

    public static void ToggleLoadingScreen(bool toggle)
    {
        OnToggleLoadingScreen?.Invoke(null, toggle);
    }
}
