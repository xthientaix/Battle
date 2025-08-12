using UnityEngine;

[CreateAssetMenu(fileName = "GlobalHealSO", menuName = "ScriptableObject/Skill/Healer/GlobalHeal")]
public class GlobalHeal : SkillData
{
    // Skill dùng lên tất cả hero

    [SerializeField] int multiHealEffect;

    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        PlayerStateManager playerStateManager = caster.GetComponent<PlayerStateManager>();
        Transform aliveHeros = playerStateManager.playerGroup.aliveHeros;
        int amountHeal = multiHealEffect * playerStateManager.heroStats.CurrentDamage;

        if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }

        foreach (Transform hero in aliveHeros)
        {
            hero.GetComponent<HeroStats>().Hited(amountHeal, DamageType.Effect, AttackType.Healer, caster);
        }
    }

    public override void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}