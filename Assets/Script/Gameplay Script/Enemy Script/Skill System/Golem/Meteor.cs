using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MeteorSO", menuName = "ScriptableObject/Skill/Golem/Meteor")]
public class Meteor : SkillData
{
    //Skill của Boss - Golem

    [SerializeField] int impactDamage;
    [SerializeField] float fallDuration;

    /*---------------------------------------------------------------------*/

    public override void Activate(GameObject effectPrefab, GameObject caster, Transform target)
    {

        if (!effectPrefab.transform.GetChild(2).TryGetComponent<MeteorEffect>(out _))
        {
            effectPrefab.transform.GetChild(2).AddComponent<MeteorEffect>();
        }
        effectPrefab.transform.GetChild(2).GetComponent<MeteorEffect>().Init(impactDamage);

        ActivateSkillEffect(effectPrefab, caster, target);
    }

    public override void ActivateSkillEffect(GameObject effectPrefab, GameObject caster, Transform target)
    {
        Vector3 position = target.position;

        //  làm cho prefab thành root object (ko có parent)
        effectPrefab.transform.parent = null;

        GameObject meteor = effectPrefab.transform.GetChild(0).gameObject;  //hiệu ứng rơi xuống
        meteor.transform.position = position + new Vector3(5 * fallDuration, 10 * fallDuration);


        GameObject shadow = effectPrefab.transform.GetChild(1).gameObject;
        shadow.transform.position = position;
        shadow.transform.localScale = new Vector3(1f, 0.5f, 0.5f);

        GameObject impactHole = effectPrefab.transform.GetChild(2).gameObject;  //hiệu ứng va chạm
        impactHole.transform.position = position + new Vector3(0f, 0f, 1.5f);
        impactHole.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        GameObject square = effectPrefab.transform.GetChild(3).gameObject;
        square.transform.parent = null;
        square.transform.position = Vector3.zero;
        square.transform.localScale = new Vector3(20, 12);

        SortingGroup sortingGroup = caster.GetComponent<SortingGroup>();
        int previousSortingIndex = sortingGroup.sortingOrder;
        sortingGroup.sortingOrder = square.GetComponent<SpriteRenderer>().sortingOrder + 1;

        meteor.SetActive(true);
        shadow.SetActive(true);
        impactHole.SetActive(false);
        square.SetActive(true);
        square.SetActive(false);
        square.SetActive(true);
        effectPrefab.SetActive(true);
        effectPrefab.SetActive(false);
        effectPrefab.SetActive(true);
        Time.timeScale = 0.3f;

        if (soundEffect != null)
        {
            caster.GetComponent<EnemyStateManager>().audioSource.clip = soundEffect;
            caster.GetComponent<EnemyStateManager>().audioSource.Play();
        }

        //tạo sequence để đồng bộ hóa hiệu ứng rơi và scale shadow
        DG.Tweening.Sequence fallSequence = DOTween.Sequence();
        fallSequence.Append(DOVirtual.DelayedCall(0.3f, () =>
        {
            Time.timeScale = 1;
            square.SetActive(false);
            square.transform.localScale = new Vector3(1, 1);
            square.transform.parent = effectPrefab.transform;
            sortingGroup.sortingOrder = previousSortingIndex;
        }, true));
        fallSequence.Append(shadow.transform.DOScale(new Vector3(2f, 1f, 1f), fallDuration).SetEase(Ease.InQuart));
        fallSequence.Join(meteor.transform.DOMove(position, fallDuration).SetEase(Ease.Linear));
        fallSequence.AppendCallback(() =>
        {
            meteor.SetActive(false);
            shadow.SetActive(false);
            impactHole.SetActive(true);
            impactHole.SetActive(false);
            impactHole.SetActive(true);
            impactHole.transform.DOScale(new Vector3(1.7f, 1.3f, 1f), 0.2f).SetEase(Ease.OutCubic);
            impactHole.transform.DOMoveZ(position.z, 0.2f);
        });
    }

    public override bool CanUse(GameObject caster, Transform target)
    {
        return true;
    }
}