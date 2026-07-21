using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BossTitleUI : MonoBehaviour
{
    private float titleDisplayTime = 4f;

    [SerializeField]
    private CanvasGroupFader titleFader;

    [SerializeField]
    private TextMeshProUGUI titleText;

    private static Action OnDisplayTitle;
    private static EventHandler<string> OnNewTitle;

    private void Start()
    {
        titleFader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        OnDisplayTitle += StartTitleDisplay;
        OnNewTitle += UpdateTitle;
    }

    private void OnDisable()
    {
        OnDisplayTitle -= StartTitleDisplay;
        OnNewTitle -= UpdateTitle;
    }

    private void StartTitleDisplay()
    {
        StartCoroutine(TimedDisplay());
    }

    private IEnumerator TimedDisplay()
    {
        titleFader.ToggleFade(true);
        yield return new WaitForSeconds(titleDisplayTime);
        titleFader.ToggleFade(false);
    }

    private void UpdateTitle(object sender, string title)
    {
        titleText.text = title;
    }

    public static void DisplayTitle()
    {
        OnDisplayTitle?.Invoke();
    }

    public static void SetNewTitle(string title)
    {
        OnNewTitle?.Invoke(null, title);
    }
}
