using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniHeroStats : MonoBehaviour, IPointerClickHandler
{
    private ShowHeroInfo showInfo;
    private Image image;
    private Sprite smallImage;
    private Sprite bigImage;

    private Tooltip tooltip;

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
    public int skill3;

    private void Awake()
    {
        showInfo = GameObject.FindGameObjectWithTag("HeroInfo").GetComponent<ShowHeroInfo>();
        image = transform.GetChild(0).GetComponent<Image>();
        showInfo.LoadHeroInfo += LoadInfo;

        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Show();
        ShowTooltip();
    }

    private void LoadInfo()
    {
        SaveLoad.LoadHeroStats(this);
        HeroStatsSO heroSO = HeroStatsHolder.instance.GetDefaultStats(type);

        if (heroSO != null)
        {
            smallImage = heroSO.smallImage;
            bigImage = heroSO.bigImage;
            image.sprite = smallImage;
        }
    }

    public void Show()
    {
        showInfo.Show(this, bigImage);
    }

    private void ShowTooltip()
    {
        string content = "";
        tooltip.EraseTooltip();
        tooltip.ShowContent(content);
    }

    public void CalculateLevel()
    {
        xpToNextLevel = (int)(100 * Mathf.Pow(1.4f, currentLevel - 1));
    }
}
