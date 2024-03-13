using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartGameState : GameStateBase
{
    private bool flag;
    private CancellationToken _ct;

    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        Debug.Log("Start");
        
    }

    public override async void OnUpdate(float deltaTime)
    {
        if (!flag)
        {
            flag = true;
            try
            {
                await UniTask.Delay(3000, DelayType.Realtime, PlayerLoopTiming.Update, _ct);
            }
            catch
            {
                Debug.LogWarning("StartCanceled");
            }

        }
        //Debug.Log("Startなう");

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStateMachine.Instance.ChangeNextState(GamePhase.Title);
            flag = false;
        }
    }

    public override void OnExit()
    {
        _ct = default;
        Debug.Log("Startおわり");
    }
}
