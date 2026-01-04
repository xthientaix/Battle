using System.Collections.Generic;
using UnityEngine;

public class EnemySkillHolder : MonoBehaviour
{
    private EnemyStateManager enemyStateManager;

    [SerializeField] private List<SkillData> skills = new();
    [SerializeField] private List<GameObject> skillEffectPrefabs = new();
    private List<float> cooldowns = new();
    //private List<float> cooldownsRemains;

    private int activeSkillIndex = -1;

    private void Awake()
    {
        enemyStateManager = gameObject.GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            cooldowns.Add(skills[i].cooldown);
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

        Invoke(nameof(UseSkill), 25f);
    }

    private void UseSkill()
    {
        enemyStateManager.UseSkillState();
    }

    //  Anim event
    public void ActiveSkill()
    {
        CancelInvoke(nameof(UseSkill));
        activeSkillIndex = Random.Range(0, skills.Count);

        Transform aliveHeros = enemyStateManager.enemyGroup.playerGroup.aliveHeros;
        Transform target = aliveHeros.GetChild(Random.Range(0, aliveHeros.childCount));

        skills[activeSkillIndex].Activate(skillEffectPrefabs[activeSkillIndex], gameObject, target);
        Invoke(nameof(UseSkill), cooldowns[activeSkillIndex] + Random.Range(0, 10));
    }
}