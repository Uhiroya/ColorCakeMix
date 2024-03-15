using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 料理フェーズ
/// </summary>
public class CookState : GameStateBase
{
    [SerializeField] private BatterManager _batterManager;
    [SerializeField] private GameObject _cookPanel;
    
    private CancellationToken _ct;
    private bool _isCooking;

    private void Awake()
    {
        _batterManager.FinishAction += OnFinishCook;
        _cookPanel.SetActive(false);
    }

    public override async void OnEnter(CancellationToken ct)
    {
        await StartCook(ct);
    }

    async UniTask StartCook(CancellationToken ct)
    {
        try
        {
            await UniTask.Delay(0, DelayType.DeltaTime, PlayerLoopTiming.Update, ct);
        }
        catch
        {
            
        }
        _cookPanel.SetActive(true);
        _isCooking = true;
        _batterManager.InitializeParameter();
        _batterManager.InitializeRandomValue();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_isCooking)
        {
            _batterManager.BakingUpdate(deltaTime);
        }
    }

    void OnFinishCook()
    {
        _isCooking = false;
        AudioManager.Instance.PlaySe(SeType.FinishCook);
        GameStateMachine.Instance.ChangeNextState(GamePhase.Judge);
    }

    public override void OnExit()
    {
        _batterManager.CalcCookedScore();
        _cookPanel.SetActive(false);
    }
}
