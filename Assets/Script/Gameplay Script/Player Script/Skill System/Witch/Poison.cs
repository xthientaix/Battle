using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonSO", menuName = "ScriptableObject/Skill/Witch/Poison")]
public class Poison : SkillData
{
    // Skill chỉ dùng lên enemy

    [SerializeField] int damagePerTick;     // Damage
    [SerializeField] int tickAmount;        // Só lần dmg/duration

    //[SerializeField] AudioClip soundEffect;

    /*-----------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        target.gameObject.AddComponent<PoisonEffect>().Init(damagePerTick, tickAmount, duration, this, effectPrefab, caster, target);

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

        effectPrefab.transform.localScale = new Vector3(0.3f, 0.3f);
        prefabSR.color = new Color(1, 1, 1, 1);
        effectPrefab.SetActive(true);

        prefabSR.DOFade(0.3f, 0.8f);
        effectPrefab.transform.DOShakePosition
            (
            duration: 0.8f,
            strength: new Vector3(0.2f, 0.2f, 0f),
            vibrato: 10,
            randomness: 90,
            fadeOut: true
            );

        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(0.7f, 0.7f), 0.8f))
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
