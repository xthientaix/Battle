using UnityEngine;

public class TauntEffect : MonoBehaviour
{
    private PlayerStateManager stateManager;

    private float endTime;

    private float shiftPerEnemy;
    private int previousArmor;
    private int previousAttackSpeed;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    public void Init(float duration, float shiftPerEnemy, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        endTime = Time.time + duration;

        this.shiftPerEnemy = shiftPerEnemy;
        this.skillData = skillData;
        this.effectPrefab = effectPrefab;
        this.caster = caster;
        this.target = target;

    }

    private void Awake()
    {
        stateManager = gameObject.GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        skillData.ActivateSkillEffect(effectPrefab, caster, target);

        Transform aliveEnemies = stateManager.playerGroup.enemyGroup.aliveEnemies;
        float percentShift = shiftPerEnemy * aliveEnemies.childCount;

        previousArmor = stateManager.heroStats.CurrentArmor;
        previousAttackSpeed = stateManager.heroStats.CurrentAttackSpeed;
        stateManager.heroStats.CurrentArmor += (int)(stateManager.heroStats.CurrentArmor * percentShift);
        stateManager.heroStats.CurrentAttackSpeed = (int)(previousAttackSpeed / (1 + percentShift / 2));

        foreach (Transform enemy in aliveEnemies)
        {
            EnemyStateManager enemyTaunted = enemy.GetComponent<EnemyStateManager>();
            enemyTaunted.target = transform;
            enemyTaunted.SwitchState(enemyTaunted.movingState);
        }
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        stateManager.heroStats.CurrentArmor = previousArmor;
        stateManager.heroStats.CurrentAttackSpeed = previousAttackSpeed;

        /*stateManager.stateColor.Remove(effectColor);
        stateManager.ChangeVisualColor(stateManager.stateColor[^1]);*/
        float attackInterval = (float)(stateManager.heroStats.CurrentAttackSpeed) / 100;
        stateManager.anim.SetFloat("AttackSpeed", attackInterval);
        Destroy(this);
    }
}
