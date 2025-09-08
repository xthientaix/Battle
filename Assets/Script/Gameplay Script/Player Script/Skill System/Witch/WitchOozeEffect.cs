using System.Collections.Generic;
using UnityEngine;

public class WitchOozeEffect : MonoBehaviour
{
    private PlayerStateManager stateManager;

    private int damagePerTick;
    private float endTime;
    private float damageTime;
    private float timePerTick;
    private float duration;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    private Color32 effectColor = new(240, 220, 80, 255);

    [SerializeField] private List<EnemyStateManager> enemiesInArea = new();

    public void Init(int damagePerTick, int tickAmount, float duration, SkillData skillData, GameObject effectPrefab, GameObject caster)
    {
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;

        this.damagePerTick = damagePerTick;
        this.duration = duration;
        timePerTick = duration / tickAmount;

        stateManager = caster.GetComponent<PlayerStateManager>();
        stateManager.heroStats.AddOnHitEffect(HitSuccessful);
        stateManager.AddOnAttack(GetTarget);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInArea.Add(collision.gameObject.GetComponent<EnemyStateManager>());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInArea.Remove(collision.gameObject.GetComponent<EnemyStateManager>());
        }
    }

    private void OnEnable()
    {
        endTime = Time.time + duration;
        damageTime = Time.time + timePerTick;
    }

    private void Update()
    {
        if (Time.time >= damageTime)
        {
            foreach (EnemyStateManager enemy in enemiesInArea)
            {
                enemy.enemyStats.Hited(damagePerTick, DamageType.Effect, AttackType.Attacker, caster);
                /*enemy.ChangeVisualColor(effectColor);
                CancelInvoke(nameof(enemy.OffColorEffet));
                Invoke(nameof(enemy.OffColorEffet), 0.5f);*/
            }

            damageTime += timePerTick;
        }

        if (Time.time < endTime)
        { return; }

        //  enemyStateManager.stateColor.Remove(effectColor);
        //  enemyStateManager.ChangeVisualColor(enemyStateManager.stateColor[^1]);

        gameObject.SetActive(false);
        enemiesInArea.Clear();
        transform.parent = caster.transform;
        transform.position = caster.transform.position;
    }

    private void GetTarget()
    {
        stateManager.RemoveOnAttack(GetTarget);
        target = stateManager.target;
    }

    private void HitSuccessful()
    {
        stateManager.heroStats.RemoveOnHitEffect(HitSuccessful);
        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }
}
