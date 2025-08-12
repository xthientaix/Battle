using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Knight,
    Wicth,
    Healer,
    Assassin
}

public enum Side
{
    Player,
    Enemy
}

public enum AttackType
{
    Attacker,
    Healer
}

public enum DamageType
{
    Normal,
    Effect
}

public class HeroStatsHolder : MonoBehaviour
{

    /*-----------------------Class Singleton-------------------------*/


    [SerializeField] private List<HeroStatsSO> defaultStatsList;

    public static HeroStatsHolder instance;

    /*--------------------------------------------------------*/

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public HeroStatsSO GetDefaultStats(HeroType type)
    {
        foreach (HeroStatsSO hero in defaultStatsList)
        {
            if (hero.type == type)
            {
                return hero;
            }
        }

        return null;
    }

    public List<HeroStatsSO> GetDefaultStatsList()
    {
        return defaultStatsList;
    }
}
