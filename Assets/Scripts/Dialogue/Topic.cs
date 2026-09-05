using System;
using UnityEngine;

public class Topic : MonoBehaviour
{
    [SerializeField]
    private bool persistingTopic = false;

    [SerializeField]
    private DialogueSO topicDialogue;

    [SerializeField]
    private TopicProgresser topicProgresser;

    private void OnEnable()
    {
        topicProgresser.OnProgressTopic += ProgressTopic;
    }

    private void OnDisable()
    {
        topicProgresser.OnProgressTopic -= ProgressTopic;
    }

    private void ProgressTopic()
    {
        if (topicDialogue)
        {
            //DialogueManager.Instance.PlayDialogue(topicDialogue, null);
            MonologueManager.Instance.AddToConversation(topicDialogue);
        }

        topicProgresser.InitialiseFollowingTopics();

        SetTopicActive(false);
    }

    public bool GetIsTopicPersistent()
    {
        return persistingTopic;
    }

    public bool GetIsTopicActive() => topicProgresser.GetIsTopicActive();

    public void SaveTopicProgress() => topicProgresser.SaveTopicProgress();

    public void LoadTopicProgress() => topicProgresser.LoadTopicProgress();

    public void SetTopicActive(bool toggle)
    {
        topicProgresser.ToggleTopicActive(toggle);
    }
}
