using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyStateManager : MonoBehaviour, ITarget
{
    private BaseState<EnemyStateManager> _currentState;
    public BaseState<EnemyStateManager> CurrentState { get { return _currentState; } }

    private BaseState<EnemyStateManager> _requiredState;
    public EnemyStats enemyStats;

    public EnemyIdleState idleState = new();
    public EnemySearchingState searchingState = new();
    public EnemyMovingState movingState = new();
    public EnemyAttackingState attackingState = new();
    public EnemyDyingState dyingState = new();

    public Vector3 locationToMove;
    public Transform target;

    public EnemyGroup enemyGroup;

    public Animator anim;

    private SortingGroup sortingGroup;

    [Header("---Sound Effect---")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip moveSound;

    [Space(10)]
    [SerializeField] private GameObject projectilePool;

    [Space(5)]
    public bool isStunned;

    [Header("---Body Visual---")]
    [SerializeField] private SpriteRenderer[] visualBody;
    public List<Color32> stateColor = new();

    [Header("---HP Bar---")]
    public Image HPBar;

    private Action attackKind;

    public Transform Target { get => target; set => target = value; }

    /*------------------------------------------------------------*/

    private void Awake()
    {
        enemyStats = transform.GetComponent<EnemyStats>();
        enemyGroup = transform.parent.GetComponentInParent<EnemyGroup>();
        audioSource = GetComponent<AudioSource>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        visualBody = GetComponentsInChildren<SpriteRenderer>();
        stateColor.Add(Color.white);

        if (enemyStats.isRange)
        {
            attackKind = ProjectilesRelease;
        }
        else
        {
            attackKind = HitTarget;
        }

        //locationToMove = transform.position + new Vector3(-5, 0, 0);

        if (_currentState == null)
        {
            _currentState = idleState;
            _currentState.EnterState(this);
        }
    }

    private void Update()
    {
        // Nếu bị stun sẽ ko chuyển state
        if (isStunned)
        { return; }

        _currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        sortingGroup.sortingOrder = -(int)(transform.position.y * 100);
    }

    public void SwitchState(BaseState<EnemyStateManager> nextState)
    {
        if (_currentState == dyingState)
        { return; }

        PlaySoundEffect("none");
        _currentState?.ExitState(this);

        if (_requiredState != null)
        {
            _currentState = _requiredState;
        }
        else
        {
            _currentState = nextState;
        }

        _currentState.EnterState(this);
        _requiredState = null;
    }

    public void RequestState(BaseState<EnemyStateManager> state)
    {
        _requiredState = state;
        SwitchState(_requiredState);
    }

    public void HitTarget()
    {
        bool isHitSuccesful = target.GetComponent<IHitable>().Hited(enemyStats.damage, DamageType.Normal, attackingState.attackType, gameObject);

        if (isHitSuccesful)
        {
            enemyStats.HitSuccessful();
        }
    }

    public void HitTarget(Transform target)
    {
        bool isHitSuccesful = target.GetComponent<IHitable>().Hited(enemyStats.damage, DamageType.Normal, attackingState.attackType, gameObject);

        if (isHitSuccesful)
        {
            enemyStats.HitSuccessful();
        }
    }

    //Anim event
    public void Attack()
    {
        PlaySoundEffect("attack");
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

    public void UpdateHealthBar(float percentRemain)
    {
        HPBar.fillAmount = percentRemain;
        if (percentRemain == 0)
        {
            HPBar.transform.parent.gameObject.SetActive(false);
        }
    }

    //Anim event
    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Dying()
    {
        SwitchState(dyingState);
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

    public void PlaySoundEffect(string action)
    {
        switch (action)
        {
            case "attack":
                {
                    if (attackSound == null)
                    {
                        return;
                    }
                    else
                    {
                        audioSource.clip = attackSound;
                        break;
                    }
                }
            case "move":
                {
                    if (moveSound == null)
                    {
                        return;
                    }
                    else
                    {
                        audioSource.clip = moveSound;
                        audioSource.loop = true;
                        break;
                    }
                }
            case "none":
                {
                    audioSource.Stop();
                    audioSource.loop = false;
                    return;
                }
        }

        audioSource.Play();
    }
}
