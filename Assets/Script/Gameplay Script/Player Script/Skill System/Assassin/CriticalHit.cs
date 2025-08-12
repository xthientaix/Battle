using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CriticalHitSO", menuName = "ScriptableObject/Skill/Assassin/CriticalHit")]
public class CriticalHit : SkillData
{
    // Skill dùng lên bản thân

    [SerializeField] private int multiDamage;
    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        caster.AddComponent<CriticalHitEffect>().Init(multiDamage, this, effectPrefab, caster);
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

        prefabSR.color = new Color(1, 1, 1, 0);
        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .Append(prefabSR.DOFade(1f, 0.2f));

        if (soundEffect != null)
        {
            caster.GetComponent<PlayerStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<PlayerStateManager>().audioSource.Play();
        }

        DOTween.Sequence().AppendInterval(0.2f)
           .OnComplete(() =>
           {
               effectPrefab.SetActive(false);
               effectPrefab.transform.parent = caster.transform;
           });
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}
