using UnityEngine;

public class CriticalHitEffect : MonoBehaviour
{
    private PlayerStateManager stateManager;
    private int previousDamage;
    private int multiDamage;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;

    public void Init(int multiDamage, SkillData skillData, GameObject effectPrefab, GameObject caster)
    {
        this.multiDamage = multiDamage;
        this.skillData = skillData;
        this.effectPrefab = effectPrefab;
        this.caster = caster;

    }

    private void Awake()
    {
        stateManager = gameObject.GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        previousDamage = stateManager.heroStats.CurrentDamage;
        stateManager.heroStats.CurrentDamage *= multiDamage;

        stateManager.heroStats.AddOnHitEffect(HitSuccessful);
    }

    private void HitSuccessful()
    {
        stateManager.heroStats.CurrentDamage = previousDamage;
        stateManager.heroStats.RemoveOnHitEffect(HitSuccessful);

        skillData.ActivateSkillEffect(effectPrefab, caster, stateManager.target);
        Destroy(this);
    }
}
