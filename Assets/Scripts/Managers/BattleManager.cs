using System;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static EventHandler<bool> OnPlayerToggle;

    private void Start()
    {
        StartCoroutine(TempPlayerEnable());
    }

    private IEnumerator TempPlayerEnable()
    {
        yield return new WaitForSeconds(0.5f);
        TogglePlayer(true);
    }

    private void TogglePlayer(bool toggle)
    {
        OnPlayerToggle?.Invoke(this, toggle);
    }
}
