using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "WitchOozeSO", menuName = "ScriptableObject/Skill/Witch/WitchOoze")]
public class WitchOoze : SkillData
{
    // Skill chỉ dùng lên enemy

    [SerializeField] int damagePerTick;     //  Damage
    [SerializeField] int tickAmount;        //  Só lần dmg/duration

    /*-----------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        effectPrefab.GetComponent<WitchOozeEffect>().Init(damagePerTick, tickAmount, duration, this, effectPrefab, caster);

        /*if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }*/
    }

    public override void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target)
    {
        if (effectPrefab == null)
        {
            Debug.Log("chưa gắn skill effect prefab");
            return;
        }

        if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }

        effectPrefab.transform.parent = null;
        effectPrefab.transform.position = target.position + new Vector3(0f, 0f, 0.5f);
        effectPrefab.transform.localScale = new Vector3(0.1f, 0.1f);

        effectPrefab.SetActive(true);
        effectPrefab.SetActive(false);
        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(1.7f, 1.2f), 0.2f).SetEase(Ease.InCubic))
            .Join(effectPrefab.transform.DOMoveZ(effectPrefab.transform.position.z - 0.5f, 0.2f));
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}
