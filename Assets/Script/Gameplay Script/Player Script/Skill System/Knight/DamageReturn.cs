using UnityEngine;

[CreateAssetMenu(fileName = "DamageReturnSO", menuName = "ScriptableObject/Skill/Knight/DamageReturn")]
public class DamageReturn : SkillData
{
    // Skill dùng lên enemy hoặc hero
    // Hiệu ứng trên đồng minh : tăng armor
    // Hiệu ứng trên kẻ địch   : 1 phần sát thương kẻ địch gây ra quay trở lại kẻ địch

    [SerializeField] private float ratio;   // tỉ lệ hiệu ứng dựa theo sát thương
    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        target.gameObject.AddComponent<DamageReturnEffect>().Init(caster, duration, ratio);
        if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }
    }

    public override void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target)
    {

    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        if (target == null)
        { return false; }

        return true;
    }
}
