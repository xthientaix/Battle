using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TauntSO", menuName = "ScriptableObject/Skill/Knight/Taunt")]
public class Taunt : SkillData
{
    [SerializeField] private float shiftPerEnemy;

    // Skill kích hoạt lên bản thân

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        caster.AddComponent<TauntEffect>().Init(duration, shiftPerEnemy, this, effectPrefab, caster, target);
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

        SpriteRenderer targetSR = caster.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer prefabSR = effectPrefab.GetComponent<SpriteRenderer>();

        effectPrefab.transform.localPosition = new Vector3(-0.05f, 0.7f);

        prefabSR.sortingLayerName = targetSR.sortingLayerName;
        prefabSR.sortingOrder = 10;

        effectPrefab.transform.localScale = new Vector3(0.2f, 0.2f);
        prefabSR.color = new Color(1, 1, 1, 1);
        effectPrefab.SetActive(true);

        DOTween.Sequence()
            .Append(effectPrefab.transform.DOScale(new Vector3(1.2f, 1.2f), 0.7f).SetEase(Ease.OutCubic))
            .Join(prefabSR.DOFade(0.5f, 0.7f))
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