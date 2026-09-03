using System;
using UnityEngine;

public class Topic : MonoBehaviour
{
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
        //Play topic dialogue
        //Evaluate next topic if applicable
    }
}
