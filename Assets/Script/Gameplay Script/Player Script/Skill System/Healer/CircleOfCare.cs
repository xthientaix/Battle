
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CircleOfCareSO", menuName = "ScriptableObject/Skill/Healer/CircleOfCare")]
public class CircleOfCare : SkillData
{
    [SerializeField] float healingAuraPercent;

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        effectPrefab.GetComponent<CircleOfCareEffect>().Init(healingAuraPercent, duration, this, effectPrefab, caster);

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

        effectPrefab.transform.parent = null;
        effectPrefab.transform.position = target.position + new Vector3(0f, 0f, 1f);
        effectPrefab.transform.localScale = new Vector3(0.1f, 0.1f);

        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(1f, 1f), 0.15f).SetEase(Ease.InCubic))
            .Join(effectPrefab.transform.DOMoveZ(effectPrefab.transform.position.z - 1f, 0.15f))
            .AppendInterval(0.1f)
            .OnComplete(() =>
            {
                effectPrefab.SetActive(false);
            });

    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}
