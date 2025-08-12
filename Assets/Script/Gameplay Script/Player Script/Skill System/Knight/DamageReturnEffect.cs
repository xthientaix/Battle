using UnityEngine;

public class DamageReturnEffect : MonoBehaviour
{
    private float endTime;

    private float ratio;
    private int previousAllyArmor;

    private HeroStats hero;
    private EnemyStats enemy;

    private GameObject caster;

    public void Init(GameObject caster, float duration, float ratio)
    {
        this.caster = caster;
        endTime = Time.time + duration;
        this.ratio = ratio;
    }

    private void Start()
    {
        // dùng skill lên hero
        if (gameObject.TryGetComponent(out hero))
        {
            previousAllyArmor = hero.CurrentArmor;
            hero.CurrentArmor += (int)(hero.CurrentArmor * ratio);

            return;
        }

        // dùng skill lên enemy
        enemy = gameObject.GetComponent<EnemyStats>();
        enemy.AddOnHitEffect(ReturnEffect);
    }

    private void Update()
    {
        if (Time.time < endTime)
        { return; }

        // dùng skill lên hero
        if (hero != null)
        {
            hero.CurrentArmor = previousAllyArmor;
        }
        else
        {
            // dùng skill lên enemy
            enemy.RemoveOnHitEffect(ReturnEffect);
        }

        Destroy(this);
    }

    private void ReturnEffect()
    {
        int returnDmg = (int)(enemy.damage * ratio);
        enemy.Hited(returnDmg, DamageType.Effect, AttackType.Attacker, caster);
    }
}
