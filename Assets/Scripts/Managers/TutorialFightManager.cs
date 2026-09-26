using System;
using UnityEngine;

public class TutorialFightManager : MonoBehaviour
{
    private bool tutorialActive = false;

    [SerializeField]
    private GameObject tutorialArenaColliders;

    [SerializeField]
    private GameObject tomeSectionBarriers;

    public static Action OnTutorialStart;
    public static EventHandler<bool> OnToggleHealthUI;
    public static EventHandler<int> OnToggleWeaponLock;
    public static EventHandler<int> OnToggleButtonPrompts;

    private void Awake()
    {
        tutorialArenaColliders.SetActive(false);
    }

    void OnDisable()
    {
        MagpieTutorialPhaseChange1.OnPhaseChangeSuccessful -= AdvanceToBowSection;
        MagpieTutorialPhaseChange2.OnPhaseChangeSuccessful -= AdvanceToTomeSection;
        MagpieTutorialPhaseChange1.OnWhirlwindReached -= EnableSpecialPrompt;
        MagpieTutorialPhaseChange3.OnToggleTomeBarriers -= ToggleTomeSectionBarriers;
    }

    public void InitiateTutorialManager(Action OnTutorialInitialised)
    {
        tutorialActive = true;

        MagpieTutorialPhaseChange1.OnPhaseChangeSuccessful += AdvanceToBowSection;
        MagpieTutorialPhaseChange2.OnPhaseChangeSuccessful += AdvanceToTomeSection;
        MagpieTutorialPhaseChange1.OnWhirlwindReached += EnableSpecialPrompt;
        MagpieTutorialPhaseChange3.OnToggleTomeBarriers += ToggleTomeSectionBarriers;

        Health playerHealth = PlayerIdentifier.PlayerTransform.GetComponent<Health>();
        playerHealth.SetUnkillable(true);

        tutorialArenaColliders.SetActive(true);
        OnToggleHealthUI?.Invoke(this, false);
        OnToggleWeaponLock?.Invoke(this, 0);
        OnToggleButtonPrompts?.Invoke(this, 0);

        OnTutorialStart?.Invoke();
        OnTutorialInitialised();
    }

    private void EnableSpecialPrompt()
    {
        if (!tutorialActive)
        {
            return;
        }

        OnToggleButtonPrompts?.Invoke(this, 1);
    }

    private void AdvanceToBowSection()
    {
        if (!tutorialActive)
        {
            return;
        }

        OnToggleWeaponLock?.Invoke(this, 1);
        OnToggleButtonPrompts?.Invoke(this, 2);
    }

    private void AdvanceToTomeSection()
    {
        if (!tutorialActive)
        {
            return;
        }

        OnToggleWeaponLock?.Invoke(this, 2);
    }

    private void ToggleTomeSectionBarriers(object sender, bool toggle)
    {
        tomeSectionBarriers.SetActive(toggle);
    }

    public void StartTutorialFinalFight(
        BossFormManager magpieBossForm,
        Action OnFinalFightInitialised
    )
    {
        Health magpieHealth = magpieBossForm.GetComponent<Health>();
        magpieHealth.SetUnkillable(false);

        OnToggleHealthUI?.Invoke(this, true);

        OnFinalFightInitialised();
    }
}
