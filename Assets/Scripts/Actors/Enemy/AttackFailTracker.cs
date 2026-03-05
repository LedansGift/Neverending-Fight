public class AttackFailTracker
{
    public AttackFailTracker(BossAttackNode attackNode, int attacksFailed)
    {
        this.attackNode = attackNode;
        this.attacksFailed = attacksFailed;
    }

    private BossAttackNode attackNode;
    private int attacksFailed;
    private bool previousAttackFail = false;

    public BossAttackNode GetAttackNode()
    {
        return attackNode;
    }

    public int GetAttacksFailed()
    {
        return attacksFailed;
    }

    public bool GetPreviousAttackFailed()
    {
        return previousAttackFail;
    }

    public void IncrementAttackFailure()
    {
        attacksFailed++;
    }

    public void SetPreviousAttackFailStatus(bool failStatus)
    {
        previousAttackFail = failStatus;
    }
}
