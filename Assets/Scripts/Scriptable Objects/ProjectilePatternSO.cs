using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectilePatternStruct
{
    public ProjectilePatternStruct(
        int projectileNumber = 5,
        float timeBetweenSpawns = 0.5f,
        float startingAngle = 0f,
        float angleChangePerSpawn = 0f,
        Vector3 startingPosition = new Vector3(),
        Vector3 positionChangePerSpawn = new Vector3(),
        int patternWaves = 1,
        Vector3 positionChangePerWave = new Vector3(),
        float timeBetweenWaves = 0.25f
    )
    {
        this.projectileNumber = projectileNumber;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.startingAngle = startingAngle;
        this.angleChangePerSpawn = angleChangePerSpawn;
        this.startingPosition = startingPosition;
        this.positionChangePerSpawn = positionChangePerSpawn;
        this.patternWaves = patternWaves;
        this.positionChangePerWave = positionChangePerWave;
        this.timeBetweenWaves = timeBetweenWaves;
    }

    public int projectileNumber;
    public float timeBetweenSpawns;
    public float startingAngle;
    public float angleChangePerSpawn;
    public Vector3 startingPosition;
    public Vector3 positionChangePerSpawn;
    public int patternWaves;
    public Vector3 positionChangePerWave;
    public float timeBetweenWaves;
    // public List<ProjectilePattern> additionalWaves = new List<ProjectilePattern>();
    // public List<float> additionalWaveDelay = new List<float>();
}

[CreateAssetMenu(fileName = "Projectile Pattern SO", menuName = "Projectile Pattern", order = 2)]
public class ProjectilePatternSO : ScriptableObject
{
    public ProjectilePatternStruct projectilePattern;
}
