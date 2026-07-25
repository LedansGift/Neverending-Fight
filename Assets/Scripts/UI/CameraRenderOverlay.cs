using System;
using UnityEngine;

public class CameraRenderOverlay : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;
    private static EventHandler<bool> OnOverlayToggle;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        OnOverlayToggle += ToggleLocalOverlay;
    }

    private void OnDisable()
    {
        OnOverlayToggle -= ToggleLocalOverlay;
    }

    private void ToggleLocalOverlay(object sender, bool toggle)
    {
        fader.ToggleFade(toggle);
    }

    public static void ToggleOverlay(bool toggle)
    {
        OnOverlayToggle?.Invoke(null, toggle);
    }
}
