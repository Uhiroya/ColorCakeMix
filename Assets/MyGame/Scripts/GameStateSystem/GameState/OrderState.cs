using System.Threading;
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
    public override void OnEnter(CancellationToken ct)
    {
        var pos = _customer.anchoredPosition;
        pos.x = _startXPosition;
        _customer.anchoredPosition = pos;
        _customer.DOAnchorPosX(0, _duration)
            .OnComplete(() =>
            {
                GameStateMachine.Instance.ChangeNextState(GamePhase.SelectMaterial);
            }).SetLink(gameObject);
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnExit()
    {

    }
}
