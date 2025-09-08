using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "BreakArmorSO", menuName = "ScriptableObject/Skill/Assassin/BreakArmor")]
public class BreakArmor : SkillData
{
    // Skill chỉ dùng lên enemy

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        target.gameObject.AddComponent<BreakArmorEffect>().Init(duration, this, effectPrefab, caster, target);

        if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }
    }

    public override void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target)
    {
        if (effectPrefab == null)
        {
            Debug.Log("chưa gắn skill effect prefab");
            return;
        }

        SpriteRenderer targetSR = target.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer prefabSR = effectPrefab.GetComponent<SpriteRenderer>();

        effectPrefab.transform.parent = target;
        effectPrefab.transform.localPosition = new Vector3(0f, 0.5f);

        prefabSR.sortingLayerName = targetSR.sortingLayerName;
        prefabSR.sortingOrder = 10;

        effectPrefab.transform.localScale = new Vector3(0.2f, 0.2f);
        prefabSR.color = new Color(1, 1, 1, 1);
        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(0.7f, 0.7f), 0.2f))
            .AppendInterval(0.15f)
            .Append(prefabSR.DOFade(0.4f, 0.2f))
            .OnComplete(() =>
            {
                effectPrefab.SetActive(false);
                effectPrefab.transform.parent = caster.transform;
            });
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        if (target == null || target.TryGetComponent<HeroStats>(out _))
        { return false; }

        return true;
    }
}
