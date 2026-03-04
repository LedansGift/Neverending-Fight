using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Pattern", menuName = "Projectile Pattern", order = 2)]
public class ProjectilePattern : ScriptableObject
{
    public int projectileNumber = 5;

    public float timeBetweenSpawns = 0.5f;
    public float startingAngle = 0f;
    public float angleChangePerSpawn = 0f;
    public Vector3 startingPosition = Vector3.zero;
    public Vector3 positionChangePerSpawn = Vector3.zero;
    public int patternWaves = 1;
    public float timeBetweenWaves = 0.25f;
    public List<ProjectilePattern> additionalWaves = new List<ProjectilePattern>();
    public List<float> additionalWaveDelay = new List<float>();
}
