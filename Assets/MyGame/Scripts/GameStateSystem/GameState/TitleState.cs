using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TitleState : GameStateBase
{
    private bool flag;
    private CancellationToken _ct;
    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        Debug.Log("Title");
        
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
                Debug.LogWarning("Canceled");
            }

        }
        //Debug.Log("Titleなう");

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStateMachine.Instance.ChangeNextState(GamePhase.StartGame);
            flag = false;
        }
    }

    public override void OnExit()
    {
        _ct = default;
        Debug.Log("Titleおわり");
    }
}
