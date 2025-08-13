using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroGroup
{
    public List<HeroType> SquadGroup = new();
    public List<HeroType> SubGroup = new();
}

public class ShowHeroInfo : MonoBehaviour
{
    [Header("Avatar")]
    [SerializeField] Image image;

    [Space(10)]
    [Header("Item")]
    public Transform weaponItemSlot;
    public Transform armorItemSlot;
    private ItemSO weaponItemSO;
    private ItemSO armorItemSO;

    [Space(10)]
    [Header("Level info")]
    [SerializeField] TextMeshProUGUI Level;
    [SerializeField] Image XPBar;
    [SerializeField] TextMeshProUGUI XPText;

    [Space(10)]
    [Header("Base stats info")]
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI armor;
    [SerializeField] TextMeshProUGUI attackSpeed;

    [Space(10)]
    [Header("Item stats info")]
    [SerializeField] TextMeshProUGUI itemHP;
    [SerializeField] TextMeshProUGUI itemDamage;
    [SerializeField] TextMeshProUGUI itemArmor;
    [SerializeField] TextMeshProUGUI itemAttackSpeed;

    [Space(10)]
    [Header("Add button")]
    [SerializeField] GameObject addButtons;
    [SerializeField] GameObject confirmAndCancelButton;

    [Space(10)]
    [Header("Heros")]
    public Transform squad;
    public Transform sub;

    private MiniHeroStats currentShow;

    public static HeroGroup heroGroup = new();

    public event Action LoadHeroInfo;

    private int[] tempStats = new int[4];
    private int[] bonusPerPoint = new int[4];
    private int tempUpgradePoint;

    private Store store;

    /*----------------------------------------------------------------------*/

    private void Awake()
    {
        store = GameObject.FindGameObjectWithTag("Store").GetComponent<Store>();
    }

    private void Start()
    {
        LoadLineup();
        GameObject.FindGameObjectWithTag("Store").GetComponent<Store>().SellItem();
        squad.GetChild(0).GetComponent<MiniHeroStats>().Show();
    }

    public void Show(MiniHeroStats hero, Sprite avatar)
    {
        currentShow = hero;

        this.image.sprite = avatar;

        weaponItemSO = ItemHolder.instance.GetItem(currentShow.weaponID);
        armorItemSO = ItemHolder.instance.GetItem(currentShow.armorID);

        weaponItemSlot.GetComponentInChildren<Item>(true).ShowItem(weaponItemSO);
        armorItemSlot.GetComponentInChildren<Item>(true).ShowItem(armorItemSO);

        ReadBaseStats();

        bonusPerPoint[0] = currentShow.HPPerPoint;
        bonusPerPoint[1] = currentShow.damagePerPoint;
        bonusPerPoint[2] = currentShow.armorPerPoint;
        bonusPerPoint[3] = currentShow.speedPerPoint;

        UpdateLevelInfo();
        UpdateBaseStatsInfo();
        UpdateItemStatsInfo();

        store.CheckItemCompatibility(currentShow.type);

        ShowAddButton(currentShow.upgradePoint);
        confirmAndCancelButton.SetActive(false);
    }

    public void SaveLineup()
    {
        heroGroup.SquadGroup.Clear();
        heroGroup.SubGroup.Clear();

        foreach (Transform hero in squad)
        {
            heroGroup.SquadGroup.Add(hero.GetComponent<MiniHeroStats>().type);
        }

        foreach (Transform hero in sub)
        {
            heroGroup.SubGroup.Add(hero.GetComponent<MiniHeroStats>().type);
        }

        string typeString = "Lineup";
        string pathString = SaveLoad.GetSavePath(typeString);
        string jsonString = JsonUtility.ToJson(heroGroup, true);
        File.WriteAllText(pathString, jsonString);
    }

    public void LoadLineup()
    {
        heroGroup.SquadGroup.Clear();
        heroGroup.SubGroup.Clear();

        string typeString = "Lineup";
        string pathString = SaveLoad.GetSavePath(typeString);

        //  có save lineup
        if (File.Exists(pathString))
        {
            string jsonString = File.ReadAllText(pathString);
            JsonUtility.FromJsonOverwrite(jsonString, heroGroup);
        }
        else
        {
            //  chưa có save lineup , tạo default lineup , lấy theo thứ tự default stats list
            for (int i = 0; i < squad.childCount; i++)
            {
                heroGroup.SquadGroup.Add(HeroStatsHolder.instance.GetDefaultStatsList()[i].type);
            }

            int squadcount = heroGroup.SquadGroup.Count;
            for (int i = 0; i < sub.childCount; i++)
            {
                heroGroup.SubGroup.Add(HeroStatsHolder.instance.GetDefaultStatsList()[i + squadcount].type);
            }
        }

        for (int i = 0; i < heroGroup.SquadGroup.Count; i++)
        {
            squad.GetChild(i).GetComponent<MiniHeroStats>().type = heroGroup.SquadGroup[i];
        }

        for (int i = 0; i < heroGroup.SubGroup.Count; i++)
        {
            sub.GetChild(i).GetComponent<MiniHeroStats>().type = heroGroup.SubGroup[i];
        }

        LoadHeroInfo?.Invoke();
    }

    public void UpdateItem()
    {
        weaponItemSO = weaponItemSlot.GetComponentInChildren<Item>().item;
        armorItemSO = armorItemSlot.GetComponentInChildren<Item>().item;

        currentShow.weaponID = weaponItemSO.ID;
        currentShow.armorID = armorItemSO.ID;

        SaveLoad.SaveHeroStats(currentShow);

        UpdateItemStatsInfo();
    }

    private void UpdateLevelInfo()
    {
        Level.text = "Lvl " + currentShow.currentLevel.ToString();
        XPText.text = currentShow.currentXP.ToString() + " / " + currentShow.xpToNextLevel.ToString();
        XPBar.fillAmount = (float)currentShow.currentXP / currentShow.xpToNextLevel;
    }

    private void UpdateBaseStatsInfo()
    {
        HP.text = tempStats[0].ToString();
        damage.text = tempStats[1].ToString();
        armor.text = tempStats[2].ToString();
        attackSpeed.text = tempStats[3].ToString();
    }

    private void UpdateItemStatsInfo()
    {
        itemHP.text = (weaponItemSO.hp + armorItemSO.hp).ToString();
        itemDamage.text = (weaponItemSO.damage + armorItemSO.damage).ToString();
        itemArmor.text = (weaponItemSO.armor + armorItemSO.armor).ToString();
        itemAttackSpeed.text = (weaponItemSO.attackSpeed + armorItemSO.attackSpeed).ToString();
    }

    public void AddStats(int index)
    {
        tempStats[index] += bonusPerPoint[index];
        tempUpgradePoint--;
        UpdateBaseStatsInfo();
        ShowAddButton(tempUpgradePoint);

        confirmAndCancelButton.SetActive(true);
    }

    private void ReadBaseStats()
    {
        tempUpgradePoint = currentShow.upgradePoint;

        tempStats[0] = currentShow.hp;
        tempStats[1] = currentShow.damage;
        tempStats[2] = currentShow.armor;
        tempStats[3] = currentShow.attackSpeed;
    }

    public void ConfirmUpGrade()
    {
        currentShow.upgradePoint = tempUpgradePoint;

        currentShow.hp = tempStats[0];
        currentShow.damage = tempStats[1];
        currentShow.armor = tempStats[2];
        currentShow.attackSpeed = tempStats[3];

        SaveLoad.SaveHeroStats(currentShow);

        confirmAndCancelButton.SetActive(false);
    }

    public void CancelUpGrade()
    {
        ReadBaseStats();
        UpdateBaseStatsInfo();
        ShowAddButton(currentShow.upgradePoint);

        confirmAndCancelButton.SetActive(false);
    }

    private void ShowAddButton(int upgradePoint)
    {
        addButtons.SetActive(upgradePoint > 0);
    }
}
