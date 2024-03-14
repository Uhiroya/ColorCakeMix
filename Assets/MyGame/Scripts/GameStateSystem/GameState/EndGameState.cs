using System.Threading;

/// <summary>
/// ゲーム終了
/// </summary>
public class EndGameState : GameStateBase
{
    private CancellationToken _ct;
    public override void OnEnter(CancellationToken ct)
    {
        // todo end game 演出
        GameStateMachine.Instance.ChangeNextState(GamePhase.Result);
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnExit()
    {

    }
}
