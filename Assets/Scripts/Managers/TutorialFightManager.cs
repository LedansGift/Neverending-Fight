using System;
using UnityEngine;

public class TutorialFightManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialArenaColliders;

    public static Action OnTutorialStart;
    public static EventHandler<bool> OnToggleHealthUI;

    private void Awake()
    {
        tutorialArenaColliders.SetActive(false);
    }

    public void InitiateTutorialManager(Action OnTutorialInitialised)
    {
        //Make player unkillable
        //Turn off player health UI
        //Enable barrier around arena
        tutorialArenaColliders.SetActive(true);
        //Lock champion bow and tome
        //Initiate UI button prompts
        OnToggleHealthUI?.Invoke(this, false);

        OnTutorialStart?.Invoke();
        OnTutorialInitialised();
    }

    public void StartTutorialFinalFight(
        BossFormManager magpieBossForm,
        Action OnFinalFightInitialised
    )
    {
        Health magpieHealth = magpieBossForm.GetComponent<Health>();
        magpieHealth.SetUnkillable(false);

        //Turn on player health UI
        OnToggleHealthUI?.Invoke(this, true);

        OnFinalFightInitialised();
    }
}
