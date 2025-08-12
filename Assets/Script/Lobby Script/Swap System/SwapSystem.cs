using DG.Tweening;
using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    [SerializeField] ShowHeroInfo showInfo;

    [Space(10)]
    [SerializeField] RectTransform selectFrame;
    [Space(10)]
    private GameObject swapableObj1;
    private GameObject swapableObj2;

    private SwapType swapableType;

    public void SelectSwapable(GameObject swapableObj, SwapType type)
    {
        bool updateSelectFramePos = true;

        if (swapableObj1 == null)
        {
            swapableObj1 = swapableObj;
            swapableType = type;
            swapableObj1.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
        else
        {
            swapableObj2 = swapableObj;

            if (CheckSwapable(type))
            {
                Swap();
                if (swapableType == SwapType.Hero)
                {
                    showInfo.SaveLineup();
                }

                if (swapableType == SwapType.Item)
                {
                    showInfo.UpdateItem();
                }

                updateSelectFramePos = false;
            }

            swapableObj1.transform.localScale = new Vector3(1f, 1f, 1);

            swapableObj1 = null;
            swapableObj2 = null;
        }

        if (!selectFrame.gameObject.activeSelf)
        { selectFrame.gameObject.SetActive(true); }

        selectFrame.localScale = swapableObj.transform.localScale;

        if (updateSelectFramePos)
        { selectFrame.position = swapableObj.transform.position; }
    }

    private void Swap()
    {
        RectTransform rect1 = swapableObj1.GetComponent<RectTransform>();
        RectTransform rect2 = swapableObj2.GetComponent<RectTransform>();

        rect1.DOAnchorPos(rect2.anchoredPosition, 0.3f);
        rect2.DOAnchorPos(rect1.anchoredPosition, 0.3f);

        Transform tempParent = swapableObj1.transform.parent;
        int index1 = swapableObj1.transform.GetSiblingIndex();
        int index2 = swapableObj2.transform.GetSiblingIndex();

        swapableObj1.transform.SetParent(swapableObj2.transform.parent);
        swapableObj2.transform.SetParent(tempParent);
        swapableObj1.transform.SetSiblingIndex(index2);
        swapableObj2.transform.SetSiblingIndex(index1);
    }

    private bool CheckSwapable(SwapType type)
    {
        if (swapableObj1 != swapableObj2 && type == swapableType)
        {
            // đk swap hero
            if (type == SwapType.Hero)
            { return true; }

            // đk swap item
            if (swapableObj1.transform.parent == showInfo.weaponItemSlot || swapableObj1.transform.parent == showInfo.armorItemSlot ||
                swapableObj2.transform.parent == showInfo.weaponItemSlot || swapableObj2.transform.parent == showInfo.armorItemSlot)
            {
                ItemSO item1 = swapableObj1.GetComponent<Item>().item;
                ItemSO item2 = swapableObj2.GetComponent<Item>().item;

                if (item1.itemType == item2.itemType &&
                    (item1.primaryType == item2.primaryType || item1.primaryType == item2.secondaryType ||
                    item1.secondaryType == item2.primaryType || item1.secondaryType == item2.secondaryType))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void OffSwapSystem()
    {
        if (swapableObj1 != null)
        { swapableObj1.transform.localScale = new Vector3(1f, 1f, 1); }
        selectFrame.gameObject.SetActive(false);

        swapableObj1 = null;
        swapableObj2 = null;
    }
}
