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
    public static BossManager Instance { get; private set; }

    private BossForm activeBossForm;

    [SerializeField]
    private BossFormManager[] bossForms;

    public static EventHandler<BossFormManager> OnNewBossForm;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
        if (activeBossForm == BossForm.CROSSROADS)
        {
            //Various shit to decide form change
            //ActivateBossForm(BossForm.MAGUS);
            FormChangeManager.Instance.ChangeBossForm(BossForm.MAGUS);
        }
        else
        {
            //End fight & reset / end game
        }

        DeactivateBossForm();
    }

    public BossForm GetActiveBossForm()
    {
        return activeBossForm;
    }

    public void ActivateTutorialBossForm()
    {
        DeactivateBossForm();
        //ActivateBossForm(BossForm.MAGPIE);
        FormChangeManager.Instance.ChangeBossForm(BossForm.MAGPIE);
    }
}
