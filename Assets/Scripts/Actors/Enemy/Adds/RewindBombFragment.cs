using UnityEngine;

public class RewindBombFragment : EnemyHealth
{
    [SerializeField]
    private GameObject fragmentVisual;

    public override void HealToFull()
    {
        fragmentVisual.SetActive(true);

        base.HealToFull();
    }

    protected override void Die()
    {
        fragmentVisual.SetActive(false);
        base.Die();
    }
}
