public class EnemyDyingState : BaseState<EnemyStateManager>
{
    public override void EnterState(EnemyStateManager stateManager)
    {
        stateManager.anim.speed = 1f;
        stateManager.anim.Play("Dying");
        stateManager.enemyGroup.EnemyDie(stateManager.transform);
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {

    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }
}
