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

    public void SpawnTopic(GameObject newTopic) { }

    //Cleanup (destroy) dead Topics at end of phase

    //On new phase start, save persisting + newly initialised topics

    //On phase reset, reinitialise saved topics
}
