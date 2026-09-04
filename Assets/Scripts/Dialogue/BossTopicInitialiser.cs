using System;
using UnityEngine;

public class BossTopicInitialiser : MonoBehaviour
{
    [SerializeField]
    private TopicSet[] initialTopics;

    public void InitialiseTopics(int activePhase)
    {
        //spawn all initial topics via topic manager
        //Do not initialise again on phase restart

        if (activePhase >= initialTopics.Length)
        {
            Debug.Log("Topic Set Does Not Exist");
            return;
        }

        initialTopics[activePhase].InitialiseTopics();
    }
}
