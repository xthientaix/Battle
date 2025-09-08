using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "BerserkSO", menuName = "ScriptableObject/Skill/Assassin/Berserk")]
public class Berserk : SkillData
{
    [SerializeField] private int multiAttackSpeed;
    [SerializeField] private float percentArmorLoss;

    // Skill kích hoạt lên bản thân

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {
        caster.AddComponent<BerserkEffect>().Init(duration, multiAttackSpeed, percentArmorLoss, this, effectPrefab, caster);
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

        //  chưa xong
        SpriteRenderer targetSR = caster.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer prefabSR = effectPrefab.GetComponent<SpriteRenderer>();

        effectPrefab.transform.localPosition = new Vector3(0f, 1.55f);

        prefabSR.sortingLayerName = targetSR.sortingLayerName;
        prefabSR.sortingOrder = 10;

        effectPrefab.transform.localScale = new Vector3(0.8f, 0.8f);
        prefabSR.color = new Color(1, 1, 1, 0.2f);
        effectPrefab.SetActive(true);

        prefabSR.DOFade(1, 0.4f).SetEase(Ease.InQuart);
        effectPrefab.transform.DOScale(0.4f, 0.4f).SetEase(Ease.InQuart);
        effectPrefab.transform.DOShakePosition
            (
            duration: 0.3f,
            strength: new Vector3(0.08f, 0.08f, 0f),
            vibrato: 5,
            randomness: 90,
            fadeOut: false
            );
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}
