using UnityEngine;

public class BreakArmorEffect : MonoBehaviour
{
    private float endTime;
    private int previousArmor;
    private EnemyStats enemyStats;
    private EnemyStateManager enemyStateManager;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;


    private Color32 effectColor = new(240, 220, 80, 255);

    public void Init(float duration, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        endTime = Time.time + duration;
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;
        this.target = target;
    }

    private void Awake()
    {
        enemyStats = gameObject.GetComponent<EnemyStats>();
        enemyStateManager = gameObject.GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        previousArmor = enemyStats.armor;
        enemyStats.armor = 0;
        enemyStateManager.stateColor.Add(effectColor);
        enemyStateManager.ChangeVisualColor(enemyStateManager.stateColor[^1]);

        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        enemyStats.armor = previousArmor;
        enemyStateManager.stateColor.Remove(effectColor);
        enemyStateManager.ChangeVisualColor(enemyStateManager.stateColor[^1]);
        Destroy(this);
    }
}
