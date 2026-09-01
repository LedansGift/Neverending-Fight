using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Random SFX", menuName = "SFX/Random SFX", order = 3)]
public class SFXObjectRandom : SFXObject
{
    public SFX[] additionalSFX;

    public override AudioSource PlaySFX(Vector3 sfxPosition, bool varyPitch = true)
    {
        List<SFX> sfxList = additionalSFX.ToList();
        sfxList.Add(soundEffect);

        int randomSFX = Random.Range(0, sfxList.Count);

        return AudioManager.PlaySFX(sfxList[randomSFX], sfxPosition, varyPitch);
    }
}
