using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    private PlayerStateManager playerStateManager;

    private List<SkillData> skills = new();
    private List<GameObject> skillEffectPrefabs = new();
    private List<float> cooldowns = new();
    private List<float> cooldownsRemains;

    [Header("---Visual Cooldown---")]
    [SerializeField] private List<Image> skillImage;
    [SerializeField] private List<Image> cooldownsImage;
    [SerializeField] private List<TextMeshProUGUI> cooldownsText;

    private int activeSkillIndex = -1;

    /*-----------------------------------------------------------------------------*/

    private void Awake()
    {
        playerStateManager = gameObject.GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        HeroStats heroStats = gameObject.GetComponent<HeroStats>();

        skills.Add(AllSkill.Instance.GetSkill(heroStats.skill1));
        skills.Add(AllSkill.Instance.GetSkill(heroStats.skill2));
        skills.Add(AllSkill.Instance.GetSkill(heroStats.skill3));

        for (int i = 0; i < skills.Count; i++)
        {
            cooldowns.Add(skills[i].cooldown);
            skillImage[i].sprite = skills[i].sprite;
        }
        cooldownsRemains = Enumerable.Repeat(0f, skills.Count).ToList();

        foreach (TextMeshProUGUI text in cooldownsText)
        {
            text.text = "";
        }

        foreach (SkillData skill in skills)
        {
            if (skill.skillEffectPrefab != null)
            {
                GameObject effectGO = Instantiate(skill.skillEffectPrefab);
                effectGO.SetActive(false);
                effectGO.transform.parent = transform;
                skillEffectPrefabs.Add(effectGO);
            }
            else
            {
                skillEffectPrefabs.Add(null);
            }
        }

        if (playerStateManager.heroStats.currentLevel < 5)
        {
            skillImage[2].transform.parent.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (cooldownsRemains[i] > 0)
            {
                cooldownsRemains[i] -= Time.deltaTime;
                if (cooldownsRemains[i] > 0)
                {
                    cooldownsText[i].text = Mathf.CeilToInt(cooldownsRemains[i]).ToString();
                }
                else
                {
                    cooldownsRemains[i] = 0;
                    cooldownsText[i].text = "";
                }

                cooldownsImage[i].fillAmount = cooldownsRemains[i] / cooldowns[i];
            }
        }
    }

    public void Skill(int index)
    {
        if (cooldownsRemains[index] > 0)
        { return; }

        if (skills[index].CanUse(gameObject, playerStateManager.target))
        {
            activeSkillIndex = index;
            playerStateManager.UseSkillState();
        }
    }

    public void ActiveSkill()
    {
        skills[activeSkillIndex].Activate(skillEffectPrefabs[activeSkillIndex], gameObject, playerStateManager.target);
        cooldownsRemains[activeSkillIndex] = cooldowns[activeSkillIndex];
    }
}
