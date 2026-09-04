using System;
using UnityEngine;

public class TopicProgresserTest : TopicProgresser
{
    [SerializeField]
    private TopicSet followingTopics;

    private void OnEnable()
    {
        PlayerGlaive.OnGlaiveSpecial += JumpTopicProgress;
    }

    private void OnDisable()
    {
        PlayerGlaive.OnGlaiveSpecial -= JumpTopicProgress;
    }

    public override void InitialiseFollowingTopics()
    {
        followingTopics?.InitialiseTopics();
    }

    private void JumpTopicProgress(object sender, bool e)
    {
        ProgressTopic();
    }
}
