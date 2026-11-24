using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    private List<int> strikeDirection = new List<int>();

    private void Start()
    {
        StartCoroutine(TestingEnable());

        // for (int i = 0; i < 10; i++)
        // {
        //     NumberTest();
        // }
    }

    private IEnumerator TestingEnable()
    {
        yield return new WaitForSeconds(0.5f);

        BossManager.Instance.ActivateBossForm(BossForm.CROSSROADS);
    }

    private void NumberTest()
    {
        strikeDirection = new List<int>();

        strikeDirection.Add(Random.Range(0, 4));

        Debug.Log("First Strike Direction = " + strikeDirection[0]);

        int adjacentNumber = (int)
            AdditionalMath.Modulus(
                strikeDirection[0] + AdditionalMath.Remap(Random.Range(0, 2), 0f, 1f, -1f, 1f),
                4f
            );

        strikeDirection.Add(adjacentNumber);

        Debug.Log("Second Strike Direction = " + strikeDirection[1]);
    }
}
