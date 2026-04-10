using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PhaseChangeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bloodEffect;

    [SerializeField]
    private CinemachineCamera bossCamera;

    private Action OnPhaseChangeCutsceneEnd;

    private void Awake()
    {
        bloodEffect.SetActive(false);
        bossCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        BossFormManager.OnPhaseChange += BossPhaseChange;
    }

    private void OnDisable()
    {
        BossFormManager.OnPhaseChange -= BossPhaseChange;
        StopAllCoroutines();
    }

    private void StartPhaseChangeCutscene(BossFormManager formManager)
    {
        //Quickly slow down music
        TimeManager.Instance.GradualPause();

        bossCamera.gameObject.SetActive(true);
        bossCamera.Target.LookAtTarget = formManager.transform;
        bossCamera.Target.TrackingTarget = formManager.transform;

        StartCoroutine(TimedPhaseChange());
    }

    private IEnumerator TimedPhaseChange()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        //Play Bossform big damage animation

        bloodEffect.SetActive(true);
        TimeManager.Instance.RestartTimeAfterGradualPause();

        yield return new WaitForSecondsRealtime(1f);

        //Boss form jumps to centre of screen

        yield return new WaitForSecondsRealtime(1f);

        bossCamera.gameObject.SetActive(false);

        if (OnPhaseChangeCutsceneEnd != null)
        {
            OnPhaseChangeCutsceneEnd();
        }
    }

    private void BossPhaseChange(object sender, Action OnPhaseChangeCutsceneEnd)
    {
        BossFormManager formManager = sender as BossFormManager;

        this.OnPhaseChangeCutsceneEnd = OnPhaseChangeCutsceneEnd;

        StartPhaseChangeCutscene(formManager);
    }
}
