using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    public ItemSO item;
    [SerializeField] private Image image;
    private Tooltip tooltip;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
    }

    public void ShowItem(ItemSO item)
    {
        this.item = item;

        if (this.item == null)
        {
            image.sprite = null;
            gameObject.SetActive(false);
        }
        else
        {
            if (this.item.sprite == null)
            {
                //Debug.Log("item SO chưa có ảnh " + transform.parent.name);
            }
            else
            {
                image.sprite = this.item.sprite;
            }
            gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string itemTypeText = "<b>- Type : </b>" + item.primaryType.ToString();
        if (item.primaryType != item.secondaryType)
        {
            itemTypeText += ", " + item.secondaryType.ToString();
        }
        string hpText = "<b>HP : </b>" + item.hp.ToString();
        string dmgText = "<b>Damage : </b>" + item.damage.ToString();
        string armorText = "<b>Armor : </b>" + item.armor.ToString();
        string attackSpeedText = "<b>Attack Speed : </b>" + item.attackSpeed.ToString();
        string content = itemTypeText + "\n" + "\t" + hpText + " , " + dmgText + " , " + armorText + " , " + attackSpeedText;
        tooltip.EraseTooltip();
        tooltip.ShowContent(content);
    }
}
