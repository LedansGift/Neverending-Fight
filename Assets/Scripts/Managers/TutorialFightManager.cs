using System;
using UnityEngine;

public class TutorialFightManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialArenaColliders;

    public static Action OnTutorialStart;
    public static EventHandler<bool> OnToggleHealthUI;
    public static EventHandler<int> OnToggleWeaponLock;

    private void Awake()
    {
        tutorialArenaColliders.SetActive(false);
    }

    public void InitiateTutorialManager(Action OnTutorialInitialised)
    {
        Health playerHealth = PlayerIdentifier.PlayerTransform.GetComponent<Health>();
        playerHealth.SetUnkillable(true);

        tutorialArenaColliders.SetActive(true);
        //Initiate UI button prompts
        OnToggleHealthUI?.Invoke(this, false);
        OnToggleWeaponLock?.Invoke(this, 0);

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
