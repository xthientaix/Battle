using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public int ID;

    [Space(10)]
    public int itemLevel;
    public HeroType primaryType;
    public HeroType secondaryType;
    public ItemType itemType;

    [Header("Image")]
    public Sprite sprite;

    [Header("Bonus Info")]
    public int hp;
    public int damage;
    public int armor;
    public int attackSpeed;
}
