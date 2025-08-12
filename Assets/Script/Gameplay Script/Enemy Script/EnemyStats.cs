using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IHitable
{
    public bool isRange;

    [Header("Xp")]
    [SerializeField] private int XPKill;
    [Space(10)]

    private int maxHP;
    public int hp;
    public int armor;
    public int damage;
    public float attackSpeed;
    public float moveSpeed;

    private EnemyStateManager stateManager;
    private XPSystem xpSystem;

    private event Action OnHitEffect;
    private event Action OnHitedEffect;

    /*-----------------------------------------------------------------------*/

    private void Awake()
    {
        stateManager = gameObject.GetComponent<EnemyStateManager>();
        xpSystem = GameObject.FindGameObjectWithTag("XPSystem").GetComponent<XPSystem>();
    }

    private void OnEnable()
    {
        maxHP = hp;
    }

    public void HitSuccessful()
    {
        OnHitEffect?.Invoke();
    }

    public bool Hited(int dmg, DamageType damageType, AttackType type, GameObject hitter)
    {
        // Mục tiêu chết trước khi bị đánh , đòn đánh ko thành
        if (!IsAlive())
        { return false; }

        if (type == AttackType.Attacker)
        {
            Damage(dmg);
        }
        else
        {
            Heal(dmg);
        }
        stateManager.UpdateHealthBar((float)hp / maxHP);

        // Mục tiêu bị đánh chết
        if (!IsAlive())
        {
            xpSystem.EnemyDie(XPKill, hitter);
            transform.GetComponent<EnemyStateManager>().Dying();
        }
        else
        {
            if (damageType == DamageType.Normal)
            {
                OnHitedEffect?.Invoke();
                stateManager.HitedEffect();
            }
        }

        if (hitter.GetComponent<HeroStats>().type == HeroType.Knight && !isRange)
        {
            stateManager.target = hitter.transform;
        }

        // Đòn đánh thành công
        return true;
    }

    public void Damage(int dmg)
    {
        int lossHP = (int)(dmg * 100 / (armor * 2 + 100));
        hp -= lossHP;

        if (hp < 0)
        {
            hp = 0;
        }
    }

    public void Heal(int dmg)
    {
        hp += dmg;

        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }

    public bool IsAlive()
    {
        if (hp == 0)
        {
            return false;
        }

        return true;
    }

    public void AddOnHitEffect(Action effect)
    {
        /*if (effect != null && (OnHitEffect == null || !Array.Exists(OnHitEffect.GetInvocationList(), method => method.Method == effect.Method)))
        {
            OnHitEffect += effect;
        }*/

        bool alreadyAdded = false;

        if (OnHitEffect != null)
        {
            alreadyAdded = Array.Exists(OnHitEffect.GetInvocationList(), method => method.Method == effect.Method);
        }

        if (effect != null && !alreadyAdded)
        {
            OnHitEffect += effect;
        }
    }

    public void RemoveOnHitEffect(Action effect)
    {
        if (effect != null)
        {
            OnHitEffect -= effect;
        }
    }

    public void AddOnHitedEffect(Action effect)
    {
        if (!Array.Exists(OnHitedEffect.GetInvocationList(), method => method.Method == effect.Method) && effect != null)
        {
            OnHitedEffect += effect;
        }
    }

    public void RemoveOnHitedEffect(Action effect)
    {
        if (effect != null)
        {
            OnHitedEffect -= effect;
        }
    }

    public void SetHP(int hp)
    {
        maxHP = hp;
        this.hp = hp;
    }
}