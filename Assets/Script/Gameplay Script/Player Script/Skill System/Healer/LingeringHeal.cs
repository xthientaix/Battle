using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "LingeringHealSO", menuName = "ScriptableObject/Skill/Healer/LingeringHeal")]
public class LingeringHeal : SkillData
{
    // Skill chỉ dùng lên hero

    [SerializeField] float percentEffect;   // Heal dựa trên %damage của healer 
    [SerializeField] int tickAmount;        // Só lần heal/duration

    //[SerializeField] AudioClip soundEffect;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        int healAmount = (int)(percentEffect * caster.GetComponent<HeroStats>().CurrentDamage);
        target.gameObject.AddComponent<LingeringHealEffect>().Init(healAmount, tickAmount, duration, this, effectPrefab, caster, target);

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
        effectPrefab.transform.localPosition = new Vector3(0.05f, 0.4f);

        prefabSR.sortingLayerName = targetSR.sortingLayerName;
        prefabSR.sortingOrder = 10;

        effectPrefab.transform.localScale = new Vector3(0.5f, 0.5f);
        prefabSR.color = new Color(1, 1, 1, 1);
        effectPrefab.SetActive(true);

        prefabSR.DOFade(0.4f, 0.6f).SetEase(Ease.InQuart);
        DOTween.Sequence()
           .Append(effectPrefab.transform.DOLocalMoveY(0.7f, 0.8f))
           .OnComplete(() =>
           {
               effectPrefab.SetActive(false);
               effectPrefab.transform.parent = caster.transform;
           });
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        if (target == null || target.TryGetComponent<EnemyStats>(out _))
        { return false; }

        return true;
    }
}
