using UnityEngine;

public class BerserkEffect : MonoBehaviour
{
    private PlayerStateManager stateManager;

    private float endTime;
    private int previousArmor;
    private float percentArmorLoss;
    private int previousAttackSpeed;
    private float multiAttackSpeed;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;

    public void Init(float duration, int multiAttackSpeed, float percentArmorLoss, SkillData skillData, GameObject effectPrefab, GameObject caster)
    {
        endTime = Time.time + duration;

        this.multiAttackSpeed = multiAttackSpeed;
        this.percentArmorLoss = percentArmorLoss;
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
        previousArmor = stateManager.heroStats.CurrentArmor;
        stateManager.heroStats.CurrentArmor = (int)(stateManager.heroStats.CurrentArmor * (1 - percentArmorLoss));

        previousAttackSpeed = stateManager.heroStats.CurrentAttackSpeed;
        stateManager.heroStats.CurrentAttackSpeed = (int)(stateManager.heroStats.CurrentAttackSpeed * multiAttackSpeed);

        skillData.ActivateSkillEffect(effectPrefab, caster, stateManager.target);
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        stateManager.heroStats.CurrentArmor = previousArmor;
        stateManager.heroStats.CurrentAttackSpeed = previousAttackSpeed;
        effectPrefab.SetActive(false);
        effectPrefab.transform.parent = caster.transform;

        /*stateManager.stateColor.Remove(effectColor);
        stateManager.ChangeVisualColor(stateManager.stateColor[^1]);*/

        float attackInterval = (float)(stateManager.heroStats.CurrentAttackSpeed) / 100;
        stateManager.anim.SetFloat("AttackSpeed", attackInterval);
        Destroy(this);
    }
}
