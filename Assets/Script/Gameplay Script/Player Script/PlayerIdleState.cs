public class PlayerIdleState : BaseState<PlayerStateManager>
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.anim.Play("Idle");
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {

    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }
}
