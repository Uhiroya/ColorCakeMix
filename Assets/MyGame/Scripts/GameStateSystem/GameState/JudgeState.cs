using System.Threading;

/// <summary>
/// 清算フェーズ
/// </summary>
public class JudgeState : GameStateBase
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
