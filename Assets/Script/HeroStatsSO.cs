using UnityEngine;

[CreateAssetMenu(fileName = "HeroStatsSO", menuName = "ScriptableObject/HeroStats")]
public class HeroStatsSO : ScriptableObject
{
    [Header("Archetype Info")]
    [TextArea]
    public string content;

    [Header("Avatar")]
    public Sprite smallImage;
    public Sprite bigImage;

    [Space(10)]
    [Header("Type")]
    public bool isRange;
    public HeroType type;
    public AttackType attackType;

    [Space(10)]
    [Header("Level")]
    public int currentLevel;
    public int currentXP;
    public int upgradePoint;
    public int xpToNextLevel;

    [Space(10)]
    [Header("Stats")]
    public int hp;
    public int armor;
    public int damage;
    public int attackSpeed;
    public float moveSpeed;

    [Space(10)]
    [Header("Bonus per Point")]
    public int HPPerPoint;
    public int damagePerPoint;
    public int armorPerPoint;
    public int speedPerPoint;

    [Space(10)]
    [Header("Item")]
    public int weaponID;
    public int armorID;

    [Space(10)]
    [Header("Skill - ID")]
    public int skill1;
    public int skill2;
}
