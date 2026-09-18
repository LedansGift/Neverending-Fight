using UnityEngine;

public class TopicProgresserIntroInterrupt : TopicProgresser
{
    //If not attacked for intro dialogue duration, progresses into different topic of "why are you not attacking me route"

    private bool introDialogueActive = false;

    [SerializeField]
    private Dialogue introDialogue;

    private void Start()
    {
        MonologueManager.Instance.AddToConversation(introDialogue, IntroDialogueEnded);
        introDialogueActive = true;
    }

    private void OnEnable()
    {
        BossHealth.OnChangeBossHealth += TryProgressTopic;
    }

    private void OnDisable()
    {
        BossHealth.OnChangeBossHealth -= TryProgressTopic;
    }

    private void TryProgressTopic(object sender, int e)
    {
        if (introDialogueActive)
        {
            ProgressTopic();
        }
    }

    private void IntroDialogueEnded()
    {
        introDialogueActive = false;
    }
}
