using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private Animator anim;
    private Vector3 offSetPosition = new(0, 0.5f, 0);

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void EndEffect()
    {
        gameObject.SetActive(false);
    }

    public void Init(Transform target)
    {
        transform.position = target.position - offSetPosition;
        gameObject.SetActive(true);
        anim.Play("Effect");
    }
}
