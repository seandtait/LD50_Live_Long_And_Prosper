using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterStat : MonoBehaviour
{
    public int healthAlterValue = 0;
    public int moneyAlterValue = 0;
    public int maxAgeAlterValue = 0;

    public IEnumerator Perform(Player _p)
    {
        if (healthAlterValue != 0)
        {
            _p.HealthChange(healthAlterValue);
            yield return null;
        }
        if (moneyAlterValue != 0)
        {
            _p.MoneyChange(moneyAlterValue);
            yield return null;
        }
        if (maxAgeAlterValue != 0)
        {
            _p.MaxAgeChange(maxAgeAlterValue);
            yield return null;
        }

    }
}
