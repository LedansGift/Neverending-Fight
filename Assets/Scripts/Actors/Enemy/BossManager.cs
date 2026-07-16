using System;
using System.Collections;
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

    public static EventHandler<BossFormManager> OnNewBossForm;

    private void Start()
    {
        //Temp
        StartCoroutine(DelayedStartEnable());
    }

    private IEnumerator DelayedStartEnable()
    {
        yield return null;
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
        activeFormManager.ActivateBossForm();

        OnNewBossForm?.Invoke(this, activeFormManager);
    }

    public void DeactivateBossForm()
    {
        BossFormManager activeFormManager = bossForms[(int)activeBossForm];
        activeFormManager.OnFinalPhaseFinished -= EvaluateNewBossForm;

        activeFormManager.DeactivateBossForm();
    }

    private void EvaluateNewBossForm()
    {
        DeactivateBossForm();
        //Various shit to decide form change
        ActivateBossForm(BossForm.MAGPIE);
    }
}
