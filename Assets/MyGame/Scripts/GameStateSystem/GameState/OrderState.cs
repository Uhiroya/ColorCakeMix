using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 注文フェーズ
/// </summary>
public class OrderState : GameStateBase
{
    [SerializeField, Tooltip("客の画像")] private RectTransform _customer;
    [SerializeField, Tooltip("客の開始地点")] private float _startXPosition;
    [SerializeField, Tooltip("客がスライドし終わるまでの時間")] private float _duration;
    private CancellationToken _ct;
    public override async void OnEnter(CancellationToken ct)
    {
        AudioManager.Instance.PlayBGM(BGMType.InGame);
        AudioManager.Instance.PlaySe(SeType.Opening);
        AudioManager.Instance.PlaySe(SeType.DoorOpen);
        AudioManager.Instance.PlaySe(SeType.FootSteps);
        //  オーダー作成
        ColorManager.Instance.DecisionOrderColor();
        //  客スライド
        var pos = _customer.anchoredPosition;
        pos.x = _startXPosition;
        _customer.anchoredPosition = pos;
        try
        {
            //  OnCompleteでステート遷移するとステート終了後でも動いてしまうのでcancellationTokenで止めるようにした
            await _customer.DOAnchorPosX(0, _duration).ToUniTask(cancellationToken: ct);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e);
            throw;
        }
        GameStateMachine.Instance.ChangeNextState(GamePhase.SelectMaterial);
        AudioManager.Instance.PlaySe(SeType.Order);
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnExit()
    {

    }
}
