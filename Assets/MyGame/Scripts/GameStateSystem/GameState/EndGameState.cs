using System.Threading;
using UnityEngine;

/// <summary>
/// ゲーム終了
/// </summary>
public class EndGameState : GameStateBase
{
    private CancellationToken _ct;
    public override void OnEnter(CancellationToken ct)
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        GameStateMachine.Instance.ChangeNextState(GamePhase.Result);
    }

    public override void OnExit()
    {

    }
}
