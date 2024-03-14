using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleState : GameStateBase
{
    [SerializeField] private CanvasGroup _titleUI; 
    [SerializeField] private Vector2 _backGroundUVSpeed;
    private CancellationToken _ct;

    public void OnClickGameStart()
    {
        GameStateMachine.Instance.ChangeNextState(GamePhase.StartGame);
    }
    
    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        _titleUI.gameObject.SetActive(true);
        BackGroundController.Instance.SetUVSpeed(_backGroundUVSpeed);
        AudioManager.Instance.PlayBGM(BGMType.Title);
        BackGroundController.Instance.Display(true);
    }

    public override void OnUpdate(float deltaTime)
    {
        BackGroundController.Instance.ManualUpdate(deltaTime);
    }

    public override void OnExit()
    {
        _ct = default;
        _titleUI.gameObject.SetActive(false);
        Debug.Log("Titleおわり");
    }
}
