using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    private VFXPlayer[] vfxPlayers;

    private Dictionary<string, VFXPlayer> vfxDictionary = new Dictionary<string, VFXPlayer>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        vfxPlayers = GetComponentsInChildren<VFXPlayer>();

        foreach (VFXPlayer player in vfxPlayers)
        {
            if (!vfxDictionary.TryAdd(player.GetVFXKey(), player))
            {
                Debug.Log("Key already exists, cannot assign: " + player.name);
            }
        }
    }

    void OnEnable()
    {
        RestartManager.OnResetPhase += DeactivateAllVFX;
    }

    void OnDisable()
    {
        RestartManager.OnResetPhase -= DeactivateAllVFX;
    }

    private IEnumerator PlayVFXDelayed(
        VFXPlayer player,
        Vector3 vfxPosition,
        Quaternion vfxRotation,
        float playDelay
    )
    {
        yield return new WaitForSeconds(playDelay);

        player.PlayVFX(vfxPosition, vfxRotation);
    }

    public void TryPlayVFX(
        string vfxKey,
        Vector3 vfxPosition,
        Quaternion vfxRotation,
        float playDelay = 0f
    )
    {
        if (!vfxDictionary.TryGetValue(vfxKey, out VFXPlayer fXPlayer))
        {
            Debug.Log("No VFX found with key: " + vfxKey);
            return;
        }

        if (playDelay <= 0f)
        {
            fXPlayer.PlayVFX(vfxPosition, vfxRotation);
        }
        else
        {
            StartCoroutine(PlayVFXDelayed(fXPlayer, vfxPosition, vfxRotation, playDelay));
        }
    }

    public void TryPlayVFX(string vfxKey, Transform vfxParent, float playDelay = 0f)
    {
        TryPlayVFX(vfxKey, vfxParent.position, vfxParent.rotation, playDelay);
    }

    public void StopVFX(string vfxKey)
    {
        if (!vfxDictionary.TryGetValue(vfxKey, out VFXPlayer fXPlayer))
        {
            Debug.Log("No VFX found with key: " + vfxKey);
            return;
        }

        Debug.Log("VFX Stopped: " + fXPlayer.name);

        fXPlayer.StopVFX();
    }

    private void DeactivateAllVFX()
    {
        StopAllCoroutines();

        foreach (VFXPlayer player in vfxPlayers)
        {
            player.StopVFX();
        }
    }
}
