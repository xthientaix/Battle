using UnityEngine;

public class PlayerGroup : MonoBehaviour
{
    public EnemyGroup enemyGroup;

    [SerializeField] Transform heroPool;
    [SerializeField] Transform firstLocations;

    [Space(10)]
    public Transform aliveHeros;
    public Transform deadHeros;

    public int aliveCount;
    private GameManager gameManager;

    private PlayerInfo _currentPlayer;
    public PlayerInfo CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }

    public int highestLevel = 1;

    /*--------------------------------------------------*/

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        Init();
    }

    public void PlayerDie(Transform player)
    {
        aliveCount--;
        player.parent = deadHeros;

        if (aliveCount == 0)
        {
            gameManager.EndGame(false);
        }
    }

    public void OffCurrentPlayerUI()
    {
        if (_currentPlayer == null)
        { return; }

        _currentPlayer.OffSkillUI();
    }

    private void Init()
    {
        SetLineup();
        GoToFirstLocation();
    }

    private void SetLineup()
    {
        foreach (HeroType heroType in ShowHeroInfo.heroGroup.SquadGroup)
        {
            HeroStats heroStats;
            foreach (Transform hero in heroPool)
            {
                heroStats = hero.GetComponent<HeroStats>();
                if (heroStats.type == heroType)
                {
                    hero.parent = aliveHeros;

                    if (heroStats.currentLevel > highestLevel)
                    {
                        highestLevel = heroStats.currentLevel;
                    }

                    break;
                }
            }
        }
        aliveCount = aliveHeros.childCount;

        foreach (Transform hero in heroPool)
        {
            hero.gameObject.SetActive(false);
        }
    }

    private void GoToFirstLocation()
    {
        for (int i = 0; i < aliveCount; i++)
        {
            PlayerStateManager stateManager = aliveHeros.GetChild(i).GetComponent<PlayerStateManager>();
            stateManager.target = null;
            stateManager.locationToMove = firstLocations.GetChild(i).position;
            stateManager.SwitchState(stateManager.movingState);
        }
    }

    public void SaveHeros()
    {
        foreach (Transform hero in aliveHeros)
        {
            SaveLoad.SaveHeroStats(hero.GetComponent<HeroStats>());
        }

        foreach (Transform hero in deadHeros)
        {
            SaveLoad.SaveHeroStats(hero.GetComponent<HeroStats>());
        }
    }
}
