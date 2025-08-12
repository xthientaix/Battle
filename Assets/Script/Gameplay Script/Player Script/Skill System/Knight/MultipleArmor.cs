using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleArmorSO", menuName = "ScriptableObject/Skill/Knight/MultipleArmor")]
public class MultipleArmor : SkillData
{
    // Skill dùng lên bản thân

    [SerializeField] private int multiTime;
    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        caster.AddComponent<MultipleArmorEffect>().Init(duration, multiTime, this, effectPrefab, caster, target);
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

        SpriteRenderer casterSR = caster.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer prefabSR = effectPrefab.GetComponent<SpriteRenderer>();

        effectPrefab.transform.localPosition = new Vector3(0f, 0.5f);

        prefabSR.sortingLayerName = casterSR.sortingLayerName;
        prefabSR.sortingOrder = 10;

        effectPrefab.transform.localScale = new Vector3(0.8f, 0.8f);
        prefabSR.color = new Color(1, 1, 1, 0.7f);
        effectPrefab.SetActive(true);

        prefabSR.DOFade(1f, 0.2f);
        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(0.5f, 0.5f), 0.25f))
            .AppendInterval(0.15f)
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
