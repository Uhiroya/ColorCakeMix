using System.Threading;

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

    }

    public override void OnExit()
    {

    }
}
