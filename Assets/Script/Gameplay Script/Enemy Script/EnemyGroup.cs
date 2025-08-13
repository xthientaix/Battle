using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public PlayerGroup playerGroup;

    [SerializeField] Transform enemyPool;
    [SerializeField] Transform firstLocations;

    [Space(10)]
    public Transform aliveEnemies;
    public Transform deadEnemies;
    public int aliveCount;
    private GameManager gameManager;

    private readonly int numberOfEnemies = 4;

    private readonly float bonusPer2Level = 0.18f;

    /*--------------------------------------------------*/

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        Init();
    }

    public void EnemyDie(Transform enemy)
    {
        aliveCount--;
        enemy.parent = deadEnemies;

        if (aliveCount <= 0)
        {
            gameManager.EndGame(true);
            return;
        }

        if (enemyPool.childCount == 0)
        {
            return;
        }

        EnemyStateManager stateManager = enemyPool.GetChild(Random.Range(0, enemyPool.childCount)).GetComponent<EnemyStateManager>();
        stateManager.gameObject.SetActive(true);

        GoToFirstLocation(stateManager, RandomFirstLocation());

    }

    private void Init()
    {
        aliveCount = enemyPool.childCount;

        foreach (Transform enemy in enemyPool)
        {
            enemy.gameObject.SetActive(false);
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (i >= aliveCount)
            { break; }

            EnemyStateManager stateManager = enemyPool.GetChild(i).GetComponent<EnemyStateManager>();
            stateManager.gameObject.SetActive(true);

            GoToFirstLocation(stateManager, firstLocations.GetChild(i).position);
        }
    }

    private void GoToFirstLocation(EnemyStateManager stateManager, Vector3 firstLocations)
    {
        int per2Level = playerGroup.highestLevel / 2;
        float bonus = per2Level * bonusPer2Level;
        int index = Random.Range(0, 2);

        switch (index)
        {
            case 0:
                stateManager.enemyStats.damage = (int)(stateManager.enemyStats.damage * (1 + bonus));
                break;
            case 1:
                stateManager.enemyStats.SetHP((int)(stateManager.enemyStats.hp * (1 + bonus)));
                break;
        }

        stateManager.target = null;
        stateManager.locationToMove = firstLocations;
        stateManager.movingState.isFirstLocation = true;
        stateManager.SwitchState(stateManager.movingState);
    }

    private Vector3 RandomFirstLocation()
    {
        return firstLocations.GetChild(Random.Range(0, firstLocations.childCount)).position;
    }

    public void AfterFirstLocation(EnemyStats enemyStats)
    {
        enemyStats.transform.parent = aliveEnemies;
    }
}
