using UnityEngine;

public class TimeSealEffect : MonoBehaviour
{
    private float endTime;

    private EnemyStateManager enemyStateManager;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    public void Init(float duration, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;
        this.target = target;

        endTime = Time.time + duration;
    }

    private void Awake()
    {
        enemyStateManager = gameObject.GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        Stun(true);
        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        Stun(false);
        Destroy(this);
    }

    private void Stun(bool isStunned)
    {
        enemyStateManager.isStunned = isStunned;
        enemyStateManager.anim.speed = (isStunned) ? 0f : 1f;
    }
}
