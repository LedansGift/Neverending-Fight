using System;
using System.Collections.Generic;
using UnityEngine;

public enum DamageZoneType
{
    circle,
    box,
    donut,
    raidwide
}

public class DamageZoneManager : MonoBehaviour
{
    private const int DEFAULT_INITIAL_ZONE_COUNT = 20;
    private const float DEFAULT_ZONE_GROW_DURATION = 0.35f;

    [SerializeField]
    private GameObject damageCircleZonePrefab;

    [SerializeField]
    private GameObject damageBoxZonePrefab;

    [SerializeField]
    private GameObject damageDonutZonePrefab;

    public static DamageZoneManager Instance { get; private set; }

    private List<List<DamageZone>> damageZones = new List<List<DamageZone>>();
    private List<int> damageZoneIndeces = new List<int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitialiseDamageZones(damageCircleZonePrefab);
        InitialiseDamageZones(damageBoxZonePrefab);
        InitialiseDamageZones(damageDonutZonePrefab);
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += DeactivateAllZones;
        BossFormManager.OnPhaseFinished += DeactivateAllZones;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= DeactivateAllZones;
        BossFormManager.OnPhaseFinished -= DeactivateAllZones;
    }

    private void InitialiseDamageZones(GameObject zonePrefab)
    {
        List<DamageZone> newZones = new List<DamageZone>();
        damageZoneIndeces.Add(0);

        for (int i = 0; i < DEFAULT_INITIAL_ZONE_COUNT; i++)
        {
            DamageZone newZone = Instantiate(zonePrefab, transform).GetComponent<DamageZone>();

            newZones.Add(newZone);
        }

        damageZones.Add(newZones);
    }

    public DamageZone SpawnDamageZone(
        Vector3 spawnLocation,
        Vector3 spawnForward,
        DamageZoneType zoneType,
        Vector2 zoneSize,
        float lifetime = 0f,
        float growDuration = DEFAULT_ZONE_GROW_DURATION
    )
    {
        int zoneListIndex = (int)zoneType;
        List<DamageZone> zones = damageZones[zoneListIndex];
        int zonesIndex = damageZoneIndeces[zoneListIndex];

        DamageZone zone = zones[zonesIndex];
        zone.ActivateZone(zoneSize, lifetime, growDuration);

        zone.transform.position = spawnLocation;
        zone.transform.forward = spawnForward;

        if ((zonesIndex + 1) >= zones.Count)
        {
            damageZoneIndeces[zoneListIndex] = 0;
        }
        else
        {
            damageZoneIndeces[zoneListIndex]++;
        }

        return zone;
    }

    private void DeactivateAllZones()
    {
        foreach (List<DamageZone> zones in damageZones)
        {
            foreach (DamageZone zone in zones)
            {
                zone.DeactivateZone(0f);
            }
        }
    }
}
