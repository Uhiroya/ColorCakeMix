using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartGameState : GameStateBase
{
    [SerializeField] private CanvasGroup _startUI; 
    [SerializeField] private Vector2 _backGroundUVSpeed;
    private bool flag;
    private CancellationToken _ct;

    public override async void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        _startUI.gameObject.SetActive(true);
        BackGroundController.Instance.SetUVSpeed(_backGroundUVSpeed);
        flag = false;
        await UniTask.Delay(1000, DelayType.Realtime, PlayerLoopTiming.Update, _ct);
        flag = true;
    }

    public override void OnUpdate(float deltaTime)
    {
        BackGroundController.Instance.ManualUpdate(deltaTime);
        if (flag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStateMachine.Instance.ChangeNextState(GamePhase.Title);
            }
        }
    }

    public override void OnExit()
    {
        _ct = default;
        _startUI.gameObject.SetActive(false);
        
    }
}
