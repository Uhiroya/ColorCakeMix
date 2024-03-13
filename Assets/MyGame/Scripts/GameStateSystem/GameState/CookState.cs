using System.Threading;

/// <summary>
/// 料理フェーズ
/// </summary>
public class CookState : GameStateBase
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
