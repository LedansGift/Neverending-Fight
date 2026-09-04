using System;
using UnityEngine;

public abstract class TopicProgresser : MonoBehaviour
{
    private bool saveTopicActive = true;

    protected bool topicActive = false;

    public Action OnProgressTopic;

    protected virtual void ProgressTopic()
    {
        if (!topicActive)
        {
            return;
        }

        OnProgressTopic?.Invoke();
    }

    public virtual void SaveTopicProgress()
    {
        saveTopicActive = topicActive;
    }

    public virtual void LoadTopicProgress()
    {
        topicActive = saveTopicActive;
    }

    public virtual void ToggleTopicActive(bool toggle)
    {
        topicActive = toggle;
    }

    public virtual void InitialiseFollowingTopics() { }

    public bool GetIsTopicActive()
    {
        return topicActive;
    }
}
