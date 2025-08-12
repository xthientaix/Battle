using UnityEngine;

public class LingeringHealEffect : MonoBehaviour
{
    private int healAmount;

    private float endTime;
    private float healTime;
    private float timePerTick;

    private HeroStats heroStats;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    public void Init(int healAmount, int tickAmount, float duration, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;
        this.target = target;

        this.healAmount = healAmount;

        timePerTick = duration / tickAmount;
        endTime = Time.time + duration;
    }

    private void Awake()
    {
        heroStats = gameObject.GetComponent<HeroStats>();
    }

    private void Start()
    {
        healTime = Time.time + timePerTick;

        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }

    private void Update()
    {
        if (Time.time >= healTime)
        {
            heroStats.Hited(healAmount, DamageType.Effect, AttackType.Healer, caster);
            healTime += timePerTick;
        }

        if (Time.time < endTime)
        { return; }

        Destroy(this);
    }
}
