using UnityEngine;

public enum BossForm
{
    CROSSROADS,
    MAGPIE,
    BARD,
    HAWK,
    MAGUS,
    DIVINE
}

public class BossManager : MonoBehaviour
{
    private BossForm activeBossForm;

    [SerializeField]
    private BossFormManager[] bossForms;

    private void Start()
    {
        //Temp
        ActivateBossForm(BossForm.CROSSROADS);
    }

    public void ActivateBossForm(BossForm bossForm)
    {
        // if (activeBossForm == bossForm)
        // {
        //     Debug.Log("New Boss Form Activation Failed. Form Already Active");
        //     return;
        // }

        activeBossForm = bossForm;

        BossFormManager activeFormManager = bossForms[(int)activeBossForm];
        activeFormManager.OnFinalPhaseFinished += EvaluateNewBossForm;
        activeFormManager.InitialiseBoss();
    }

    public void DeactivateBossForm()
    {
        BossFormManager activeFormManager = bossForms[(int)activeBossForm];
        activeFormManager.OnFinalPhaseFinished -= EvaluateNewBossForm;
    }

    private void EvaluateNewBossForm()
    {
        //Various shit to decide form change
    }
}
