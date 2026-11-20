using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormChangeManager : MonoBehaviour
{
    private int activeCutsceneIndex = 0;
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

        formCutsceneMap.Add(BossForm.MAGPIE, 0);
        formCutsceneMap.Add(BossForm.MAGUS, 1);
    }

    public void ChangeBossForm(BossForm newForm, Transform playerStartTransform)
    {
        activeFormChange = newForm;

        if (formCutsceneMap.TryGetValue(newForm, out int cutsceneIndex))
        {
            StartCoroutine(FormChangeSetup(cutsceneIndex, playerStartTransform));
        }
        else
        {
            StartCoroutine(FormChange(newForm, playerStartTransform));
        }
    }

    private IEnumerator FormChangeSetup(int cutsceneIndex, Transform playerStartTransform)
    {
        LoadingScreenUI.ToggleLoadingScreen(true);

        yield return new WaitForSeconds(2.5f);

        SetPlayerDefaultPosition(playerStartTransform);

        formChangeCutscenes[cutsceneIndex].InitialiseCutsceneHandler(
            activeFormChange,
            arenaManager
        );

        LoadingScreenUI.ToggleLoadingScreen(false);

        //Temp hack for including TextCutscenes without changing too much structure \/

        formCutsceneMap.TryGetValue(BossForm.MAGPIE, out int magpieIndex);

        activeCutsceneIndex = cutsceneIndex;

        if (magpieIndex == cutsceneIndex)
        {
            TextCutsceneUI.StartTextCutsceneEvent(3, StartFormChangeAfterTextCutscene);
        }
        else
        {
            TextCutsceneUI.StartTextCutsceneEvent(6, StartFormChangeAfterTextCutscene);
        }
    }

    private void SetPlayerDefaultPosition(Transform playerStartTransform)
    {
        PlayerMovement playerMovement =
            PlayerIdentifier.PlayerTransform.GetComponent<PlayerMovement>();

        if (playerMovement)
        {
            playerMovement.SetPlayerTransform(playerStartTransform);
        }
    }

    // private void StartTutorialFormChangeCutscene()
    // {
    //     formCutsceneMap.TryGetValue(BossForm.MAGPIE, out int cutsceneIndex);
    //     StartFormChangeCutscene(cutsceneIndex);
    // }

    private void StartFormChangeAfterTextCutscene()
    {
        StartFormChangeCutscene(activeCutsceneIndex);
    }

    private void StartFormChangeCutscene(int cutsceneIndex)
    {
        CutsceneManager.Instance.StartCutscene(
            formChangeCutscenes[cutsceneIndex].GetCutsceneDirector(),
            FinaliseFormChange
        );
    }

    //Default form change when there's no cutscene

    private IEnumerator FormChange(BossForm newForm, Transform playerStartTransform)
    {
        LoadingScreenUI.ToggleLoadingScreen(true);

        yield return new WaitForSeconds(2.5f);

        //if (newForm != BossForm.MAGPIE) { }

        arenaManager.SwitchArena(newForm);

        //Set player position to be in set arena position
        SetPlayerDefaultPosition(playerStartTransform);

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
