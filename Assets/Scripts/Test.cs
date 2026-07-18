using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TestingEnable());
    }

    private IEnumerator TestingEnable()
    {
        yield return new WaitForSeconds(0.5f);

        BossManager.Instance.ActivateBossForm(BossForm.CROSSROADS);
    }
}
