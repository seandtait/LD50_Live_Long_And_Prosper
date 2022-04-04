using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainAchiev : MonoBehaviour
{
    public int achievIndex;

    public IEnumerator Perform(Player _p)
    {
        _p.GetAchievement(achievIndex);
        yield return null;
    }
}
