using System;
using System.Collections.Generic;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance { get; private set; }

    private bool conversationActive = false;
    private Dialogue activeDialogue;
    private Action dialogueEndAction;
    private Queue<Dialogue> conversationQueue = new Queue<Dialogue>();
    private Queue<Action> dialogueEndQueue = new Queue<Action>();

    [SerializeField]
    private DialogueManager dialogueManager;

    //private Action OnDialogueEnd;

    public static EventHandler<bool> OnConversationActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void AdvanceConversation()
    {
        if (conversationQueue.TryDequeue(out Dialogue newDialogue))
        {
            activeDialogue = newDialogue;
            dialogueEndAction = dialogueEndQueue.Dequeue();

            dialogueManager.PlayDialogue(newDialogue, EndCurrentDialogue);
        }
        else
        {
            EndConversation();
        }
    }

    private void EndConversation()
    {
        conversationActive = false;

        //OnConversationActive?.Invoke(this, false);

        // if (OnDialogueEnd != null)
        // {
        //     OnDialogueEnd();
        //     OnDialogueEnd = null;
        // }
    }

    private void EndCurrentDialogue()
    {
        TryInvokeDialogueEndAction();
        AdvanceConversation();
    }

    private void TryInvokeDialogueEndAction()
    {
        if (dialogueEndAction != null)
        {
            dialogueEndAction();
            dialogueEndAction = null;
        }
    }

    public void AddToConversation(Dialogue newDialogue, Action onDialogueEnd = null)
    {
        conversationQueue.Enqueue(newDialogue);
        dialogueEndQueue.Enqueue(onDialogueEnd);

        //OnDialogueEnd = onDialogueEnd;

        if (!conversationActive)
        {
            conversationActive = true;
            //OnConversationActive?.Invoke(this, true);
            AdvanceConversation();
        }
    }

    public void InterruptConversation(Dialogue newDialogue, Action onDialogueEnd = null)
    {
        TryInvokeDialogueEndAction();

        dialogueEndAction = onDialogueEnd;

        activeDialogue = newDialogue;
        dialogueManager.InterruptDialogue(newDialogue, EndCurrentDialogue);
    }
}
