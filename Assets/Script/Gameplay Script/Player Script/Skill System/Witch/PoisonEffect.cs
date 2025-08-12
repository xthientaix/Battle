using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private int damagePerTick;

    private float endTime;
    private float damageTime;
    private float timePerTick;

    private EnemyStats enemyStats;
    private EnemyStateManager enemyStateManager;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    private Color32 effectColor = new(240, 220, 80, 255);

    public void Init(int damagePerTick, int tickAmount, float duration, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;
        this.target = target;

        this.damagePerTick = damagePerTick;

        timePerTick = duration / tickAmount;
        endTime = Time.time + duration;
    }

    private void Awake()
    {
        enemyStats = gameObject.GetComponent<EnemyStats>();
        enemyStateManager = gameObject.GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        damageTime = Time.time + timePerTick;
        enemyStateManager.stateColor.Add(effectColor);
        enemyStateManager.ChangeVisualColor(enemyStateManager.stateColor[^1]);

        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }

    private void Update()
    {
        if (Time.time >= damageTime)
        {
            enemyStats.Hited(damagePerTick, DamageType.Effect, AttackType.Attacker, caster);
            damageTime += timePerTick;
        }

        if (Time.time < endTime)
        { return; }

        enemyStateManager.stateColor.Remove(effectColor);
        enemyStateManager.ChangeVisualColor(enemyStateManager.stateColor[^1]);
        Destroy(this);
    }
}
