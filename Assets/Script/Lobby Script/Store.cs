using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] List<Transform> storePlots;
    [SerializeField] List<ItemSO> items;
    private ShowHeroInfo showInfo;

    private void Awake()
    {
        showInfo = GameObject.FindGameObjectWithTag("HeroInfo").GetComponent<ShowHeroInfo>();
    }

    public void SellItem()
    {
        if (GameManager.isMatchCompleted)
        {
            items.Clear();

            foreach (Transform hero in showInfo.squad)
            {
                items.Add(SearchItem(hero.GetComponent<MiniHeroStats>()));
            }

            GameManager.isMatchCompleted = false;
        }

        ShowItems();
    }

    private ItemSO SearchItem(MiniHeroStats hero)
    {
        HeroType heroType = hero.type;
        int enumCount = Enum.GetValues(typeof(ItemType)).Length;
        ItemType randomItemType = (ItemType)UnityEngine.Random.Range(0, enumCount);
        int itemlevel = hero.currentLevel / 2;

        List<ItemSO> itemList = ItemHolder.instance.GetItemList(heroType, randomItemType, itemlevel);

        return itemList[UnityEngine.Random.Range(0, itemList.Count)];
    }

    private void ShowItems()
    {
        for (int i = 0; i < storePlots.Count; i++)
        {
            int available = items.Count;
            if (i < available)
            {
                storePlots[i].GetComponentInChildren<Item>(true).ShowItem(items[i]);
            }
            else
            {
                storePlots[i].GetComponentInChildren<Item>(true).ShowItem(null);
            }
        }
    }

    public void CheckItemCompatibility(HeroType heroType)
    {
        Item item;
        for (int i = 0; i < items.Count; i++)
        {
            item = storePlots[i].GetComponentInChildren<Item>();
            bool canSwap = (item.item.primaryType == heroType || item.item.secondaryType == heroType);
            storePlots[i].GetComponentInChildren<Swapable>().SetCanSwap(canSwap);
        }
    }
}
