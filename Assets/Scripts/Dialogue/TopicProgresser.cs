using System;
using UnityEngine;

public class TopicProgresser : MonoBehaviour
{
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

    public virtual void ToggleTopicActive(bool toggle)
    {
        topicActive = toggle;
    }
}
