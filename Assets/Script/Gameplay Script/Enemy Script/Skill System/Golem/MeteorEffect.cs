using UnityEngine;


public class MeteorEffect : MonoBehaviour
{
    private int impactDamage;
    private bool canDmg = false;
    private bool firstEnable = true;

    public void Init(int impactDamage)
    {
        this.impactDamage = impactDamage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!canDmg)
        {
            return;
        }

        if (collision.CompareTag("PlayerFoot"))
        {
            collision.transform.parent.GetComponent<PlayerStateManager>().heroStats.Hited(impactDamage, DamageType.Effect, AttackType.Attacker, null);
        }
    }

    private void OnEnable()
    {
        if (firstEnable)
        {
            firstEnable = false;
            return; // bỏ qua lần đầu
        }

        canDmg = true;
        Invoke(nameof(OffDmg), 0.4f);
        Invoke(nameof(OffEffect), 1.5f);
    }

    private void OffEffect()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void OffDmg()
    {
        canDmg = false;
    }
}