using System;
using UnityEngine;

public class HeroStats : MonoBehaviour, IHitable
{
    [Space(10)]
    [Header("Type")]
    public bool isRange;
    public HeroType type;
    public AttackType attackType;

    [Space(10)]
    [Header("Level")]
    public int currentLevel = 1;
    public int currentXP;
    public int upgradePoint;
    public int xpToNextLevel;

    [Space(10)]
    [Header("Stats")]
    public int hp;
    public int armor;
    public int damage;
    public int attackSpeed;
    public float moveSpeed;

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    private int currentArmor;
    private int currentDamage;
    private int currentAttackSpeed;

    [Space(10)]
    [Header("Bonus per Point")]
    public int HPPerPoint;
    public int damagePerPoint;
    public int armorPerPoint;
    public int speedPerPoint;

    [Space(10)]
    [Header("Skill - ID")]
    public int skill1;
    public int skill2;

    [Space(10)]
    [Header("Item")]
    public int weaponID;
    public int armorID;
    private ItemSO weaponItem;
    private ItemSO armorItem;

    private PlayerStateManager stateManager;
    private XPSystem xpSystem;

    private event Action OnHitEffect;
    private event Action OnHitedEffect;

    /*--------------------------------------------------------------------------*/

    private void Awake()
    {
        stateManager = gameObject.GetComponent<PlayerStateManager>();

        xpSystem = GameObject.FindGameObjectWithTag("XPSystem").GetComponent<XPSystem>();
        xpSystem.GainXPAction += GainXP;

        // Load chỉ số hero
        SaveLoad.LoadHeroStats(this);

        // Lấy thong tin item
        weaponItem = ItemHolder.instance.GetItem(weaponID);
        armorItem = ItemHolder.instance.GetItem(armorID);
    }

    private void OnEnable()
    {
        maxHP = hp;
        currentArmor = armor;
        currentDamage = damage;
        currentAttackSpeed = attackSpeed;
        if (weaponItem != null)
        {
            maxHP += weaponItem.hp;
            currentArmor += weaponItem.armor;
            currentDamage += weaponItem.damage;
            currentAttackSpeed += weaponItem.attackSpeed;
        }

        if (armorItem != null)
        {
            maxHP += armorItem.hp;
            currentArmor += armorItem.armor;
            currentDamage += armorItem.damage;
            currentAttackSpeed += armorItem.attackSpeed;
        }

        currentHP = maxHP;
    }

    public void HitSuccessful()
    {
        OnHitEffect?.Invoke();
    }

    public bool Hited(int dmg, DamageType damageType, AttackType type, GameObject hitter)
    {
        // Mục tiêu chết trước khi đánh , đòn đánh ko thành
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
        stateManager.UpdateHealthBar((float)currentHP / maxHP);

        if (!IsAlive())
        {
            xpSystem.GainXPAction -= GainXP;
            transform.GetComponent<PlayerStateManager>().Dying();
        }
        else
        {
            if (type == AttackType.Healer)
            {
                stateManager.HealedEffect();
            }
            else
            {
                if (damageType == DamageType.Normal)
                {
                    OnHitedEffect?.Invoke();
                    stateManager.HitedEffect();
                }
            }
        }

        return true;
    }

    public void Damage(int dmg)
    {
        int lossHP = (int)(dmg * 40 / (currentArmor + 40));
        currentHP -= lossHP;

        if (currentHP < 0)
        {
            currentHP = 0;
        }
    }

    public void Heal(int dmg)
    {
        currentHP += dmg;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public bool IsAlive()
    {
        if (currentHP == 0)
        {
            return false;
        }

        return true;
    }

    private void GainXP(int xp, GameObject hitter)
    {
        if (currentLevel == 10)
        {
            return;
        }

        if (hitter == gameObject)
        {
            currentXP += xp;
        }
        else
        {
            if (type == HeroType.Healer)
            {
                currentXP += (int)(xp * 0.6);
            }
            else
            {
                currentXP += (int)(xp * 0.5);
            }
        }

        if (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            upgradePoint += 2;
            CalculateLevel();
        }
    }

    public int CurrentDamage
    {
        get { return currentDamage; }
        set { currentDamage = (int)value; }
    }

    public int CurrentArmor
    {
        get { return currentArmor; }
        set { currentArmor = (int)value; }
    }

    public int CurrentAttackSpeed
    {
        get { return currentAttackSpeed; }
        set { currentAttackSpeed = value; }
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

    public void CalculateLevel()
    {
        xpToNextLevel = (int)(100 * Mathf.Pow(1.4f, currentLevel - 1));
    }
}