using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 清算フェーズ
/// </summary>
public class JudgeState : GameStateBase
{
    [SerializeField] private GameObject _judgePanel;
    [SerializeField] private Image _judgeCakeImage;
    [SerializeField] private Text _judgePriceText;
    [SerializeField, Tooltip("一定時間後に強制Orderに戻る")] private float _judgeTime;
    
    private CancellationToken _ct;
    private float _judgeTimer;
    
    public override void OnEnter(CancellationToken ct)
    {
        _judgeTimer = 0;
        _judgePanel.SetActive(true);
        InGameManager.CakeData judgeCakeData = InGameManager.Instance.SaveFinishedCake();
        _judgeCakeImage.color = judgeCakeData.Color;
        _judgePriceText.text = judgeCakeData.Price + "円";
    }

    public override void OnUpdate(float deltaTime)
    {
        _judgeTimer += deltaTime;

        if (_judgeTimer >= _judgeTime)
        {
            GameStateMachine.Instance.ChangeNextState(GamePhase.Order);
        }
    }

    public override void OnExit()
    {
        _judgePanel.SetActive(false);
    }
}
