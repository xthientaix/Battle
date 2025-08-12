using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerStateManager : MonoBehaviour, ITarget
{
    private BaseState<PlayerStateManager> _currentState;

    public HeroStats heroStats;

    public PlayerIdleState idleState = new();
    public PlayerSearchingState searchingState = new();
    public PlayerMovingState movingState = new();
    public PlayerAttackingState attackingState = new();
    public PlayerUsingSkillState usingSkillState = new();
    public PlayerDyingState dyingState = new();

    public Vector3 locationToMove;
    public Transform target;

    public PlayerGroup playerGroup;

    public Animator anim;

    private SortingGroup sortingGroup;

    [Header("---Sound Effect---")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip moveSound;

    [Space(10)]
    [SerializeField] private GameObject projectilePool;

    [Space(10)]
    [SerializeField] private HitEffect hitEffect;  // Hiệu ứng hình ảnh khi projectile trúng mục tiêu , có thể null (ko có effect)

    [Header("---Body Visual---")]
    [SerializeField] private SpriteRenderer[] visualBody;
    public List<Color32> stateColor = new();

    [Header("---HP Bar---")]
    public Image HPBar;

    private Action attackKind;
    private SkillHolder skillHolder;

    public Transform Target { get => target; set => target = value; }

    /*--------------------------------------------------------*/

    private void Awake()
    {
        heroStats = transform.GetComponent<HeroStats>();
        playerGroup = transform.parent.GetComponentInParent<PlayerGroup>();
        skillHolder = transform.GetComponent<SkillHolder>();
        audioSource = GetComponent<AudioSource>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        visualBody = GetComponentsInChildren<SpriteRenderer>();
        stateColor.Add(Color.white);

        if (heroStats.isRange && (heroStats.attackType == AttackType.Attacker))
        {
            attackKind = ProjectilesRelease;
        }
        else
        {
            attackKind = HitTarget;
        }

        if (hitEffect != null)
            hitEffect.gameObject.SetActive(false);

        if (_currentState == null)
        {
            _currentState = idleState;
            _currentState.EnterState(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        sortingGroup.sortingOrder = -(int)(transform.position.y * 100);
    }

    public void SwitchState(BaseState<PlayerStateManager> nextState)
    {
        if (_currentState == dyingState)
        { return; }

        _currentState?.ExitState(this);
        _currentState = nextState;
        _currentState.EnterState(this);
    }

    public void HitTarget()
    {
        bool isHitSuccesful = target.GetComponent<IHitable>().Hited(heroStats.CurrentDamage, DamageType.Normal, attackingState.attackType, gameObject);

        if (isHitSuccesful)
        {
            if (hitEffect != null)
                hitEffect.Init(target);

            heroStats.HitSuccessful();
        }
    }

    // Dùng cho projectile của range hero
    public void HitTarget(Transform target)
    {
        bool isHitSuccesful = target.GetComponent<IHitable>().Hited(heroStats.CurrentDamage, DamageType.Normal, attackingState.attackType, gameObject);

        if (isHitSuccesful)
        {
            if (hitEffect != null)
                hitEffect.Init(target);

            heroStats.HitSuccessful();
        }
    }

    // Anim event
    public void Attack()
    {
        if (attackSound != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
        }
        attackKind.Invoke();
    }

    public void ProjectilesRelease()
    {
        foreach (Transform projectile in projectilePool.transform)
        {
            if (!projectile.gameObject.activeSelf)
            {
                projectile.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void UseSkillState()
    {
        if (_currentState != usingSkillState)
        {
            usingSkillState.previousState = _currentState;
        }

        SwitchState(usingSkillState);
    }

    // Anim event
    public void ChanneledSkill()
    {
        //skillHolder.ActiveSkill();
        usingSkillState.isChanneling = false;
    }

    public void UpdateHealthBar(float percentRemain)
    {
        HPBar.fillAmount = percentRemain;
        if (percentRemain == 0f)
        {
            HPBar.transform.parent.gameObject.SetActive(false);
        }
    }

    // Anim event
    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Dying()
    {
        SwitchState(dyingState);
    }

    public void ChangeTarget(Transform target)
    {
        // ***    Lưu ý : có truyền null làm tham số    *** 

        this.target = target;
        if (target != null)
            locationToMove = target.position;
    }

    public void HitedEffect()
    {
        ChangeVisualColor(new Color32(255, 130, 130, 255));

        CancelInvoke(nameof(OffColorEffet));
        Invoke(nameof(OffColorEffet), 0.5f);
    }

    public void HealedEffect()
    {
        ChangeVisualColor(new Color32(150, 255, 200, 255));

        CancelInvoke(nameof(OffColorEffet));
        Invoke(nameof(OffColorEffet), 0.5f);
    }

    private void OffColorEffet()
    {
        ChangeVisualColor(stateColor[^1]);
    }

    public void ChangeVisualColor(Color32 color)
    {
        // Đổi màu tất cả Sprite con
        foreach (SpriteRenderer bodyPart in visualBody)
        {
            bodyPart.color = color;
        }
    }
}
