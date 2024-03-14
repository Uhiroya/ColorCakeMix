using System;
using System.Threading;
using Cinemachine;
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

    public override void OnEnter(CancellationToken ct)
    {
        Invoke(nameof(StartCook), 2);
        _cookPanel.SetActive(true);
    }

    void StartCook()
    {
        _isCooking = true;
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
        GameStateMachine.Instance.ChangeNextState(GamePhase.Judge);
    }

    public override void OnExit()
    {
        _batterManager.CalcCookedScore();
        _cookPanel.SetActive(false);
    }
}
