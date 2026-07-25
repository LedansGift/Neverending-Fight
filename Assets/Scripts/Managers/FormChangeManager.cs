using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormChangeManager : MonoBehaviour
{
    private BossForm activeFormChange;

    public static FormChangeManager Instance { get; private set; }

    private Dictionary<BossForm, int> formCutsceneMap = new Dictionary<BossForm, int>();

    [SerializeField]
    private ArenaManager arenaManager;

    [SerializeField]
    private FormChangeCutsceneHandler[] formChangeCutscenes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        formCutsceneMap.Add(BossForm.MAGUS, 0);
    }

    public void ChangeBossForm(BossForm newForm)
    {
        activeFormChange = newForm;

        if (formCutsceneMap.TryGetValue(newForm, out int cutsceneIndex))
        {
            formChangeCutscenes[cutsceneIndex].InitialiseCutsceneHandler(
                activeFormChange,
                arenaManager
            );
            StartFormChangeCutscene(cutsceneIndex);
        }
        else
        {
            StartCoroutine(FormChange(newForm));
        }
    }

    private void StartFormChangeCutscene(int cutsceneIndex)
    {
        CutsceneManager.Instance.StartCutscene(
            formChangeCutscenes[cutsceneIndex].GetCutsceneDirector(),
            FinaliseFormChange
        );
    }

    private IEnumerator FormChange(BossForm newForm)
    {
        LoadingScreenUI.ToggleLoadingScreen(true);

        yield return new WaitForSeconds(2.5f);

        arenaManager.SwitchArena(newForm);
        //Set player position to be in set arena position

        LoadingScreenUI.ToggleLoadingScreen(false);
        yield return new WaitForSeconds(2.5f);

        FinaliseFormChange();
    }

    private void FinaliseFormChange()
    {
        BossManager.Instance.ActivateBossForm(activeFormChange);
        BattleManager.Instance.TogglePlayer(true);
    }
}
