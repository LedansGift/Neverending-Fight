using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    private float dialogueFadeTime = 0.25f;
    private float defaultDisplayTime = 2f;

    private Coroutine dialogueDisplayCoroutine;

    [SerializeField]
    private CanvasGroupFader dialogueFader;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        ClearDialogueText();
        dialogueFader.SetCanvasGroupAlpha(0f);
        dialogueFader.ToggleBlockRaycasts(false);
    }

    private void Start()
    {
        DialogueManager.Instance.OnDialogue += DialogueManager_OnDialogue;
    }

    private void OnDisable()
    {
        DialogueManager.Instance.OnDialogue -= DialogueManager_OnDialogue;
    }

    private void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private IEnumerator DisplaySentence(DialogueUIEventArgs dialogueUIEventArgs)
    {
        dialogueText.text = dialogueUIEventArgs.sentence;

        dialogueFader.ToggleFade(true);

        float displayDuration = dialogueUIEventArgs.displayDuration;

        if (displayDuration < 0f)
        {
            displayDuration = defaultDisplayTime;
        }

        yield return new WaitForSeconds(displayDuration - dialogueFadeTime);

        dialogueFader.ToggleFade(false);

        yield return new WaitForSeconds(dialogueFadeTime);
    }

    private void DialogueManager_OnDialogue(object sender, DialogueUIEventArgs dialogueArgs)
    {
        if (dialogueDisplayCoroutine != null)
        {
            StopCoroutine(dialogueDisplayCoroutine);
        }

        dialogueDisplayCoroutine = StartCoroutine(DisplaySentence(dialogueArgs));
    }
}
