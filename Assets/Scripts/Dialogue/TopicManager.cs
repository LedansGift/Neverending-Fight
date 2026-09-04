using System.Collections.Generic;
using UnityEngine;

public class TopicManager : MonoBehaviour
{
    public static TopicManager Instance { get; private set; }

    private List<Topic> activeTopics;
    private List<Topic> savedTopics;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        activeTopics = new List<Topic>();
        savedTopics = new List<Topic>();
    }

    private void CleanupInactiveTopics()
    {
        List<Topic> newSavedList = new List<Topic>();

        foreach (Topic topic in savedTopics)
        {
            if (!topic.GetIsTopicActive())
            {
                Destroy(topic.gameObject);
            }
            else
            {
                newSavedList.Add(topic);
            }
        }

        foreach (Topic topic in activeTopics)
        {
            if (!topic.GetIsTopicActive() || !topic.GetIsTopicPersistent())
            {
                Destroy(topic.gameObject);
            }
            else
            {
                newSavedList.Add(topic);
            }
        }

        savedTopics = newSavedList;
        activeTopics = new List<Topic>();
    }

    private void ResetSavedTopicsProgress()
    {
        foreach (Topic topic in savedTopics)
        {
            topic.LoadTopicProgress();
        }
    }

    private void SaveActiveTopicsProgress()
    {
        foreach (Topic topic in savedTopics)
        {
            topic.SaveTopicProgress();
        }
    }

    public void SpawnTopic(Topic newTopic)
    {
        GameObject spawnedTopicObject = Instantiate(newTopic.gameObject, transform);
        Topic spawnedTopic = spawnedTopicObject.GetComponent<Topic>();
        spawnedTopic.SetTopicActive(true);

        activeTopics.Add(spawnedTopic);
    }

    public void ResetActiveTopics()
    {
        foreach (Topic topic in activeTopics)
        {
            Destroy(topic.gameObject);
        }

        activeTopics = new List<Topic>();
        ResetSavedTopicsProgress();
    }

    public void AdvancePhase()
    {
        CleanupInactiveTopics();
        SaveActiveTopicsProgress();
    }
}
