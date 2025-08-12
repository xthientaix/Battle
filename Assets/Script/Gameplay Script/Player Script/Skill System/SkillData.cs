using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public int id;

    [Space(10)]
    [TextArea]
    public string tooltipContent;

    [Space(10)]
    public Sprite sprite;
    public string skillName;
    public float cooldown;
    public float duration;

    public AudioClip soundEffect;
    public GameObject skillEffectPrefab;

    public abstract bool CanUse(GameObject caster, Transform target);

    public abstract void Activate(GameObject effectPrefab, GameObject caster, Transform target);

    public abstract void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target);

}
