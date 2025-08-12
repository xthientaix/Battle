using System;
using System.Collections.Generic;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public static readonly List<int> Level = new() { 0, 100, 250, 450 };
    public Action<int, GameObject> GainXPAction;

    public void EnemyDie(int xp, GameObject hitter)
    {
        GainXPAction?.Invoke(xp, hitter);
    }
}
