using UnityEngine;

public class TopicSet : MonoBehaviour
{
    [SerializeField]
    private Topic[] childTopics;

    public Topic[] GetTopics()
    {
        return childTopics;
    }

    public void InitialiseTopics()
    {
        foreach (Topic topic in childTopics)
        {
            TopicManager.Instance.SpawnTopic(topic);
        }
    }
}
