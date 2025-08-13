using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BasicHeroInfo : MonoBehaviour
{
    [SerializeField] Tooltip tooltip;

    [SerializeField] private List<Image> heroAvatar;
    [SerializeField] private List<Image> heroSkillImage;
    [SerializeField] private GameObject level5Lock;

    private List<HeroStatsSO> listHero;
    private List<SkillData> heroSkill = new() { null, null, null };
    private List<int> heroLevel = new();

    /*------------------------------------------------------------------*/

    private void Start()
    {
        listHero = HeroStatsHolder.instance.GetDefaultStatsList();

        for (int i = 0; i < heroAvatar.Count; i++)
        {
            heroAvatar[i].sprite = listHero[i].smallImage;
            heroLevel.Add(GetHeroLevel(listHero[i].type));
        }

        tooltip.EraseTooltip();
        ClickOnHero(0);
    }

    public void ClickOnHero(int index)
    {
        heroSkill[0] = AllSkill.Instance.GetSkill(listHero[index].skill1);
        heroSkill[1] = AllSkill.Instance.GetSkill(listHero[index].skill2);
        heroSkill[2] = AllSkill.Instance.GetSkill(listHero[index].skill3);

        heroSkillImage[0].sprite = heroSkill[0].sprite;
        heroSkillImage[1].sprite = heroSkill[1].sprite;
        heroSkillImage[2].sprite = heroSkill[2].sprite;

        level5Lock.SetActive((heroLevel[index] < 5));

        tooltip.ShowContent(listHero[index].content);
    }

    public void ClickOnSkill(int index)
    {
        tooltip.ShowContent(heroSkill[index].tooltipContent);
    }

    private int GetHeroLevel(HeroType type)
    {
        int level = 1;

        string pathString = SaveLoad.GetSavePath(type.ToString());
        if (File.Exists(pathString))
        {
            string json = File.ReadAllText(pathString);
            var node = JSON.Parse(json);

            level = node["currentLevel"].AsInt;
        }

        return level;
    }
}
