using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeSealSO", menuName = "ScriptableObject/Skill/Witch/TimeSeal")]
public class TimeSeal : SkillData
{
    // Skill chỉ dùng lên enemy

    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        target.gameObject.AddComponent<TimeSealEffect>().Init(duration, this, effectPrefab, caster, target);

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

        effectPrefab.transform.localScale = new Vector3(0.5f, 0.5f);
        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .AppendInterval(duration)
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
