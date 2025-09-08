using UnityEngine;

public class CircleOfCareEffect : MonoBehaviour
{
    private PlayerStateManager stateManager;
    private Transform orderTransform;
    SpriteRenderer prefabSR;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    private float healPercent;

    public void Init(float healingAuraPercent, float duration, SkillData skillData, GameObject effectPrefab, GameObject caster)
    {
        this.skillData = skillData;
        this.caster = caster;
        this.effectPrefab = effectPrefab;
        healPercent = healingAuraPercent;

        stateManager = caster.GetComponent<PlayerStateManager>();
        stateManager.heroStats.AddOnHitEffect(HitSuccessful);
        stateManager.AddOnAttack(GetTarget);

        Invoke(nameof(OffEffect), duration);
    }

    private void Awake()
    {
        orderTransform = transform.GetChild(0).GetComponent<Transform>();
        prefabSR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject != target)
        {
            int healAmount = (int)(stateManager.heroStats.CurrentDamage * healPercent);
            collision.gameObject.GetComponent<HeroStats>().Hited(healAmount, DamageType.Effect, AttackType.Healer, stateManager.gameObject);
        }
    }

    private void LateUpdate()
    {
        prefabSR.sortingOrder = -(int)(orderTransform.position.y * 100);
    }

    private void GetTarget()
    {
        //stateManager.RemoveOnAttack(GetTarget);
        target = stateManager.target;
    }

    private void HitSuccessful()
    {
        skillData.ActivateSkillEffect(effectPrefab, caster, stateManager.target);
    }

    private void OffEffect()
    {
        stateManager.RemoveOnAttack(GetTarget);
        stateManager.heroStats.RemoveOnHitEffect(HitSuccessful);
    }
}
