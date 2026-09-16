using System;
using UnityEngine;

public class BossReactionUI : MonoBehaviour
{
    [SerializeField]
    private Animator reactionAnimator;

    // [SerializeField]
    // private ParticleSystem successParticles;

    // [SerializeField]
    // private ParticleSystem failParticles;

    private void Start()
    {
        BossAttackManager.OnAttackFailed += TriggerReaction;
    }

    private void OnDisable()
    {
        BossAttackManager.OnAttackFailed -= TriggerReaction;
    }

    private void TriggerReaction(object sender, bool attackFailed)
    {
        if (attackFailed)
        {
            reactionAnimator.SetTrigger("fail");
        }
        else
        {
            reactionAnimator.SetTrigger("success");
        }
    }
}
