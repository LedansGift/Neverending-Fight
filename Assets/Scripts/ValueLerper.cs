using System.Collections.Generic;
using UnityEngine;

struct LerpInfo
{
    public float valueStart;
    public float valueEnd;
    public float lerpSpeed;
}

public class ValueLerper : MonoBehaviour
{
    // public static ValueLerper Instance { get; private set; }

    // private List<float> lerpingValues = new List<float>();
    // private List<LerpInfo> lerpingInfos = new List<LerpInfo>();

    // private void Awake()
    // {
    //     if (Instance != null)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     Instance = this;
    // }

    // private void Update()
    // {
    //     for (int i = 0; i < lerpingValues.Count; i++)
    //     {
    //         float lerpValue = lerpingValues[i];
    //         lerpValue += Time.deltaTime;
    //     }
    // }

    // public void LerpValue(ref float value, float valueStart, float valueEnd, float lerpSpeed = 1f)
    // {
    //     lerpingValues.Add(value);
    // }
}
