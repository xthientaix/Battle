using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicHeroInfo : MonoBehaviour
{
    [SerializeField] Tooltip tooltip;

    [SerializeField] private List<Image> heroAvatar;
    [SerializeField] private List<Image> heroSkillImage;

    private List<HeroStatsSO> listHero;
    private List<SkillData> heroSkill = new() { null, null };

    /*------------------------------------------------------------------*/

    private void Start()
    {
        listHero = HeroStatsHolder.instance.GetDefaultStatsList();

        for (int i = 0; i < heroAvatar.Count; i++)
        {
            heroAvatar[i].sprite = listHero[i].smallImage;
        }

        tooltip.EraseTooltip();
        ClickOnHero(0);
    }

    public void ClickOnHero(int index)
    {
        heroSkill[0] = AllSkill.Instance.GetSkill(listHero[index].skill1);
        heroSkill[1] = AllSkill.Instance.GetSkill(listHero[index].skill2);

        heroSkillImage[0].sprite = heroSkill[0].sprite;
        heroSkillImage[1].sprite = heroSkill[1].sprite;

        tooltip.ShowContent(listHero[index].content);
    }

    public void ClickOnSkill(int index)
    {
        tooltip.ShowContent(heroSkill[index].tooltipContent);
    }
}
