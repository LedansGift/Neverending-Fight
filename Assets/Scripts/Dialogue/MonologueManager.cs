using System;
using System.Collections.Generic;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance { get; private set; }

    private bool conversationActive = false;
    private DialogueSO activeDialogue;
    private Queue<DialogueSO> conversationQueue = new Queue<DialogueSO>();

    [SerializeField]
    private DialogueManager dialogueManager;

    private Action OnDialogueEnd;

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
        if (conversationQueue.TryDequeue(out DialogueSO newDialogue))
        {
            activeDialogue = newDialogue;

            dialogueManager.PlayDialogue(newDialogue, AdvanceConversation);
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

        if (OnDialogueEnd != null)
        {
            OnDialogueEnd();
            OnDialogueEnd = null;
        }
    }

    public void AddToConversation(DialogueSO newDialogue, Action onDialogueEnd = null)
    {
        conversationQueue.Enqueue(newDialogue);

        OnDialogueEnd = onDialogueEnd;

        if (!conversationActive)
        {
            conversationActive = true;
            //OnConversationActive?.Invoke(this, true);
            AdvanceConversation();
        }
    }
}
