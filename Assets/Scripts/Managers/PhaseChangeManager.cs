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

    [SerializeField]
    private BossMoveNode centreMoveNode;

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

        bloodEffect.transform.position = formManager.transform.position;

        StartCoroutine(TimedPhaseChange(formManager));
    }

    private IEnumerator TimedPhaseChange(BossFormManager formManager)
    {
        yield return new WaitForSecondsRealtime(1.5f);

        formManager.PlayBossDamagedAnimation();

        bloodEffect.SetActive(true);
        TimeManager.Instance.RestartTimeAfterGradualPause();

        //Play UI animation of player getting healed up & recovering retries

        yield return new WaitForSecondsRealtime(4f);

        //Boss form jumps to centre of screen
        formManager.GetBossAttackManager().PerformAttackNode(centreMoveNode, null);

        yield return new WaitForSecondsRealtime(3f);

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
