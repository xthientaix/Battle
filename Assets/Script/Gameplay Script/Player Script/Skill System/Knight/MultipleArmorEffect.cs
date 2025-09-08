using UnityEngine;

public class MultipleArmorEffect : MonoBehaviour
{
    private float endTime;
    private int previousArmor;
    private HeroStats heroStats;
    private PlayerStateManager stateManager;

    private float multiTime;

    private SkillData skillData;
    private GameObject effectPrefab;
    private GameObject caster;
    private Transform target;

    private Color32 effectColor = new(150, 220, 255, 255);

    public void Init(float duration, float multiTime, SkillData skillData, GameObject effectPrefab, GameObject caster, Transform target)
    {
        endTime = Time.time + duration;
        this.multiTime = multiTime;
        this.skillData = skillData;
        this.effectPrefab = effectPrefab;
        this.caster = caster;
        this.target = target;
    }

    private void Awake()
    {
        heroStats = gameObject.GetComponent<HeroStats>();
        stateManager = gameObject.GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        previousArmor = heroStats.CurrentArmor;
        heroStats.CurrentArmor = (int)(heroStats.CurrentArmor * multiTime);

        stateManager.stateColor.Add(effectColor);
        stateManager.ChangeVisualColor(stateManager.stateColor[^1]);

        skillData.ActivateSkillEffect(effectPrefab, caster, target);
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        heroStats.CurrentArmor = previousArmor;
        stateManager.stateColor.Remove(effectColor);
        stateManager.ChangeVisualColor(stateManager.stateColor[^1]);
        Destroy(this);
    }
}
