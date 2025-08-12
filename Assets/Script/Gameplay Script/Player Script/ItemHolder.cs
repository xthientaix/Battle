using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private List<ItemSO> weaponList;
    [SerializeField] private List<ItemSO> armorList;

    public static ItemHolder instance;

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

    public ItemSO GetItem(int ID)
    {
        if (ID >= 50)
        {
            foreach (ItemSO item in armorList)
            {
                if (ID == item.ID)
                {
                    return item;
                }
            }
        }
        else
        {
            foreach (ItemSO item in weaponList)
            {
                if (ID == item.ID)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public List<ItemSO> GetItemList(HeroType heroType, ItemType itemType, int itemLevelRequest)
    {
        List<ItemSO> itemList = new();
        List<ItemSO> requestedList = new();

        switch (itemType)
        {
            case ItemType.Weapon:
                {
                    itemList = weaponList;
                    break;
                }
            case ItemType.Armor:
                {
                    itemList = armorList;
                    break;
                }
        }

        foreach (ItemSO item in itemList)
        {
            // Lấy những item có level bằng hoặc thấp hơn yêu cầu 1 level
            if (item.primaryType == heroType && (item.itemLevel == itemLevelRequest || item.itemLevel == (itemLevelRequest - 1)))
            {
                requestedList.Add(item);
            }
        }

        return requestedList;
    }
}
