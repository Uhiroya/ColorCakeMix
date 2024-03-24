using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 料理フェーズ
/// </summary>
public class CookState : GameStateBase
{
    [SerializeField] private BatterManager _batterManager;
    [SerializeField] private GameObject _cookPanel;
    [SerializeField, Range(0, 20), Tooltip("矢印が点滅する回数")] private int _arrowYoyoCount = 5;
    [SerializeField, Tooltip("左矢印")] private Image _directionArrowLeft;
    [SerializeField, Tooltip("右矢印")] private Image _directionArrowRight;
    [SerializeField] private CinemachineVirtualCamera _cookingCamera;
    [SerializeField] private CinemachineVirtualCamera _orderCamera;
    
    private CancellationToken _ct;
    private bool _isCooking;
    private UniTask.Awaiter _awaiter;

    private void Awake()
    {
        _batterManager.FinishAction += OnFinishCook;
        _cookPanel.SetActive(false);
    }

    public override async void OnEnter(CancellationToken ct)
    {
        var alphaRight = _directionArrowRight.color;
        alphaRight.a = 0;
        _directionArrowRight.color = alphaRight;
        var alphaLeft = _directionArrowLeft.color;
        alphaLeft.a = 0;
        _directionArrowLeft.color = alphaLeft;
        await StartCook(ct);
    }

    async UniTask StartCook(CancellationToken ct)
    {
        try
        {
            _cookingCamera.Priority = 1;
            _orderCamera.Priority = 0;
            //  カメラのブレンド速度が2秒なので2000tick待つ
            await UniTask.Delay(2000, DelayType.DeltaTime, PlayerLoopTiming.Update, ct);
            _cookPanel.SetActive(true);
            _isCooking = true;
            _batterManager.InitializeParameter();
            _batterManager.InitializeRandomValue();
            await RotateNavigation(ct);
            _batterManager.BatterRotater.OppositeDirection
                .SkipLatestValueOnSubscribe().Subscribe(b =>
                {
                    if (b && _awaiter.IsCompleted)
                    {
                        _awaiter = RotateNavigation(ct).GetAwaiter();
                    }
                }).AddTo(ct);
        }
        catch
        {
            
        }
    }
    /// <summary>
    /// 回す方向をプレイヤーに教える処理
    /// </summary>
    async UniTask RotateNavigation(CancellationToken ct)
    {
        var image = _batterManager.BatterRotater.RotateDirection ? 
            _directionArrowRight : _directionArrowLeft;
        var alpha = image.color;
        alpha.a = 1;
        image.color = alpha;
        await image.DOFade(0, 0.3f)
                .SetLoops(_arrowYoyoCount * 2 - 1, LoopType.Yoyo)
                .WithCancellation(ct);
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
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
