public class PlayerDyingState : BaseState<PlayerStateManager>
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.anim.Play("Dying");
        stateManager.playerGroup.PlayerDie(stateManager.transform);
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {

    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }
}
