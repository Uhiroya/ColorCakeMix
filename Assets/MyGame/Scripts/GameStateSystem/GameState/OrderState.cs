using System.Threading;

/// <summary>
/// 注文フェーズ
/// </summary>
public class OrderState : GameStateBase
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
