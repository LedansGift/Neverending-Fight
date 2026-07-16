using UnityEngine;

public class CameraBossFollower : CameraFollower
{
    private Transform activeBossTransform;

    protected override void Start()
    {
        base.Start();
        BossManager.OnNewBossForm += UpdateBossTarget;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BossManager.OnNewBossForm -= UpdateBossTarget;
    }

    protected override void SetTarget()
    {
        if (!activeBossTransform)
        {
            return;
        }

        followTransform = activeBossTransform;
    }

    private void UpdateBossTarget(object sender, BossFormManager newForm)
    {
        activeBossTransform = newForm.transform;
        SetTarget();
    }
}
